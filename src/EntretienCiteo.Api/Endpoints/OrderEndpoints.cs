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
        group.MapGet("/{id:int}", GetOrderById);

        return app;
    }

    // POST /orders
    private async static Task<IResult> CreateOrder(AppDbContext context, CreateOrderRequest request)
    {
        // TODO:
        // Feedback + correction
        // Valider que le client existe
        // valider stock avant de créer
        // Gérer le stock des produits commandés
        // Créer avec le statut Pending

        var customers = await context.Customers.ToListAsync();

        if (!customers.Any(c => c.Id == request.CustomerId))
            return Results.BadRequest($"Customer with id {request.CustomerId} does not exist.");

        var a = 0;

        var products = await context.Products.ToListAsync();

        var outOfStockProducts = new List<int>();

        foreach (var line in request.Lines)
        {
            if (products.FirstOrDefault(_ => _.Id == line.ProductId)?.Stock < line.Quantity)
            {
                outOfStockProducts.Add(line.ProductId);
            }
        }

        if (outOfStockProducts.Count != 0)
            return Results.BadRequest($"Stock unavailable for products with ids: {string.Join(", ", outOfStockProducts)}");

        var order = new Order
        {
            CustomerId = request.CustomerId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Lines = [.. request.Lines.Select(l => new OrderLine
            {
                ProductId = l.ProductId,
                Quantity = l.Quantity,
                UnitPrice = products[l.ProductId].Price
            })]
        };

        for (int i = 0; i < request.Lines.Count; i++)
        {
            var line = request.Lines[i];
            var product = products.First(p => p.Id == line.ProductId);
            product.Stock -= line.Quantity;
        }

        context.Orders.Add(order);
        context.SaveChanges();

        var orderAmount = 10;

        return Results.Created(
            $"/orders/{order.Id}",
            new OrderDetail(
                order.Id,
                request.CustomerId,
                "Nom du Client",
                order.Status.ToString(),
                order.CreatedAt,
                Lines: [],
                orderAmount));
    }

    // GET /orders/{id}
    private static async Task<IResult> GetOrderById(int id)
    {
        //Renvoyer la commande correspondante via le DTO OrderDetail
        return Results.Ok();
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