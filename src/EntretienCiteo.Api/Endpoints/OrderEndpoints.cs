using EntretienCiteo.Api.Data;
using EntretienCiteo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntretienCiteo.Api.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        group.MapPost("/", CreateOrder);
        group.MapPut("/{id:int}/confirm", ConfirmOrder);
        group.MapGet("/{id:int}", GetOrderById);

        return app;
    }

    // POST /orders
    private static async Task<IResult> CreateOrder(CreateOrderRequest request, AppDbContext db)
    {
        if (request.Lines is null || request.Lines.Count == 0)
            return Results.BadRequest("Order must contain at least one line.");

        var customerExists = await db.Customers.AnyAsync(c => c.Id == request.CustomerId);
        if (!customerExists)
            return Results.NotFound($"Customer {request.CustomerId} not found.");

        var productIds = request.Lines.Select(l => l.ProductId).Distinct().ToList();
        var products = await db.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        var missingProducts = productIds.Except(products.Select(p => p.Id)).ToList();
        if (missingProducts.Count > 0)
            return Results.NotFound($"Products not found: {string.Join(", ", missingProducts)}");

        // Validate stock before creating anything
        var stockErrors = new List<string>();
        foreach (var line in request.Lines)
        {
            var product = products.First(p => p.Id == line.ProductId);
            if (product.Stock < line.Quantity)
                stockErrors.Add($"Product '{product.Name}' has only {product.Stock} units in stock (requested {line.Quantity}).");
        }

        if (stockErrors.Count > 0)
            return Results.UnprocessableEntity(new { Errors = stockErrors });

        var order = new Order
        {
            CustomerId = request.CustomerId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Lines = request.Lines.Select(l =>
            {
                var product = products.First(p => p.Id == l.ProductId);
                return new OrderLine
                {
                    ProductId = l.ProductId,
                    Quantity = l.Quantity,
                    UnitPrice = product.Price  // snapshot price at creation
                };
            }).ToList()
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", await BuildOrderDetail(db, order.Id));
    }

    // PUT /orders/{id}/confirm
    private static async Task<IResult> ConfirmOrder(int id, AppDbContext db)
    {
        var order = await db.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
            return Results.NotFound();

        if (order.Status != OrderStatus.Pending)
            return Results.Conflict($"Order is in status '{order.Status}' and cannot be confirmed.");

        // Decrement stock with optimistic concurrency — retry on conflict
        const int maxRetries = 3;
        for (var attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                foreach (var line in order.Lines)
                {
                    if (line.Product.Stock < line.Quantity)
                        return Results.UnprocessableEntity(
                            $"Product '{line.Product.Name}' no longer has sufficient stock.");

                    line.Product.Stock -= line.Quantity;
                    line.Product.RowVersion = Guid.NewGuid(); // bump token to detect concurrent conflicts
                }

                order.Status = OrderStatus.Confirmed;
                await db.SaveChangesAsync();

                return Results.Ok(await BuildOrderDetail(db, order.Id));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (attempt == maxRetries - 1)
                    return Results.Conflict("Could not confirm order due to concurrent modifications. Please retry.");

                // Refresh stale entities and retry
                foreach (var entry in ex.Entries)
                    await entry.ReloadAsync();
            }
        }

        return Results.StatusCode(500);
    }

    // GET /orders/{id}
    private static async Task<IResult> GetOrderById(int id, AppDbContext db)
    {
        var detail = await BuildOrderDetail(db, id);
        return detail is null ? Results.NotFound() : Results.Ok(detail);
    }

    private static async Task<OrderDetail?> BuildOrderDetail(AppDbContext db, int orderId)
    {
        var order = await db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null) return null;

        return new OrderDetail(
            order.Id,
            order.CustomerId,
            order.Customer.Name,
            order.Status.ToString(),
            order.CreatedAt,
            order.Lines.Select(l => new OrderLineDetail(
                l.ProductId,
                l.Product.Name,
                l.Quantity,
                l.UnitPrice,
                l.UnitPrice * l.Quantity
            )).ToList(),
            order.Total
        );
    }

    // DTOs
    public record CreateOrderRequest(int CustomerId, List<OrderLineRequest> Lines);
    public record OrderLineRequest(int ProductId, int Quantity);
    public record OrderDetail(
        int Id,
        int CustomerId,
        string CustomerName,
        string Status,
        DateTime CreatedAt,
        List<OrderLineDetail> Lines,
        decimal Total
    );
    public record OrderLineDetail(int ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal LineTotal);
}
