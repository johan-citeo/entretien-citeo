using EntretienCiteo.Api.Data;
using EntretienCiteo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntretienCiteo.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        group.MapGet("/", GetProducts);
        group.MapGet("/{id:int}", GetProductById);

        return app;
    }

    // GET /products?page=1&pageSize=10&categoryId=1&minPrice=10&maxPrice=500
    private static IResult GetProducts(
        int page = 1,
        int pageSize = 10,
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        var items = new List<ProductListItem>();
        return Results.Ok(new PagedResult<ProductListItem>(items, 1, page, pageSize));
    }

    // GET /products/{id}
    private static IResult GetProductById(int id)
    {
        //Renvoyer un ProductDetail
        return Results.Ok();
    }

    // DTOs
    public record ProductListItem(int Id, string Name, decimal Price, int Stock, string CategoryName);
    public record ProductDetail(int Id, string Name, decimal Price, int Stock, CategoryInfo Category);
    public record CategoryInfo(int Id, string Name, CategoryInfo? Parent);
    public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);
}
