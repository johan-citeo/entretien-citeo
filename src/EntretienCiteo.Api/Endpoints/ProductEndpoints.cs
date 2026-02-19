namespace EntretienCiteo.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        group.MapGet("/{id:int}", GetProductById);
        group.MapGet("/", GetProducts);

        return app;
    }

    // GET /products/{id}
    private static IResult GetProductById(int id)
    {
        //Renvoyer le produit correspondant via le DTO ProductDetail
        return Results.Ok();
    }

    // GET /products?page=1&pageSize=10&categoryId=1&minPrice=10&maxPrice=500
    private static IResult GetProducts(
        int page = 1,
        int pageSize = 10,
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        var items = new List<ProductDetail>();
        return Results.Ok(new PagedResult<ProductDetail>(items, 1, page, pageSize));
    }

    // DTOs
    public record ProductDetail(int Id, string Name, decimal Price, int Stock, CategoryInfo Category);
    public record CategoryInfo(int Id, string Name);
    public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);
}