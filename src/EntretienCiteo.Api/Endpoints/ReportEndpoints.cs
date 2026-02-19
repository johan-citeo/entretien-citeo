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
        int top = 10,
        DateTime? from = null,
        DateTime? to = null)
    {
        var results = new List<TopProductResult>();

        return Results.Ok(results);
    }

    // DTOs
    public record TopProductResult(int ProductId, string Name, decimal Revenue, int QuantitySold);
}
