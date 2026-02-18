using EntretienCiteo.Api.Data;
using EntretienCiteo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntretienCiteo.Api.Endpoints;

public static class ReportEndpoints
{
    public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/reports").WithTags("Reports");

        group.MapGet("/top-products", GetTopProducts);

        return app;
    }

    // GET /reports/top-products?top=10&from=2024-01-01&to=2024-12-31
    private static async Task<IResult> GetTopProducts(
        AppDbContext db,
        int top = 10,
        DateTime? from = null,
        DateTime? to = null)
    {
        if (top < 1 || top > 100)
            return Results.BadRequest("top must be between 1 and 100.");

        if (from.HasValue && to.HasValue && from > to)
            return Results.BadRequest("'from' must be before 'to'.");

        var query = db.OrderLines
            .AsNoTracking()
            .Where(l => l.Order.Status == OrderStatus.Confirmed || l.Order.Status == OrderStatus.Shipped);

        if (from.HasValue)
            query = query.Where(l => l.Order.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(l => l.Order.CreatedAt <= to.Value);

        var results = await query
            .GroupBy(l => new { l.ProductId, l.Product.Name })
            .Select(g => new TopProductResult(
                g.Key.ProductId,
                g.Key.Name,
                g.Sum(l => l.UnitPrice * l.Quantity),
                g.Sum(l => l.Quantity)
            ))
            .OrderByDescending(r => r.Revenue)
            .Take(top)
            .ToListAsync();

        return Results.Ok(results);
    }

    // DTOs
    public record TopProductResult(int ProductId, string Name, decimal Revenue, int QuantitySold);
}
