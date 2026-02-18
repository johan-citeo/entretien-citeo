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
    private static async Task<IResult> GetProducts(
        AppDbContext db,
        int page = 1,
        int pageSize = 10,
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
            return Results.BadRequest("page must be >= 1 and pageSize between 1 and 100.");

        // Collect category IDs including all subcategories recursively
        IReadOnlySet<int> categoryFilter = categoryId.HasValue
            ? await GetCategoryIdsRecursive(db, categoryId.Value)
            : new HashSet<int>();

        var query = db.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .AsQueryable();

        if (categoryFilter.Count > 0)
            query = query.Where(p => categoryFilter.Contains(p.CategoryId));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductListItem(p.Id, p.Name, p.Price, p.Stock, p.Category.Name))
            .ToListAsync();

        return Results.Ok(new PagedResult<ProductListItem>(items, total, page, pageSize));
    }

    // GET /products/{id}
    private static async Task<IResult> GetProductById(int id, AppDbContext db)
    {
        var product = await db.Products
            .Include(p => p.Category)
            .ThenInclude(c => c.ParentCategory)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return Results.NotFound();

        var result = new ProductDetail(
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            new CategoryInfo(
                product.Category.Id,
                product.Category.Name,
                product.Category.ParentCategory is null ? null
                    : new CategoryInfo(product.Category.ParentCategory.Id, product.Category.ParentCategory.Name, null)
            )
        );

        return Results.Ok(result);
    }

    private static async Task<HashSet<int>> GetCategoryIdsRecursive(AppDbContext db, int rootId)
    {
        var result = new HashSet<int>();
        var queue = new Queue<int>();
        queue.Enqueue(rootId);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (!result.Add(current)) continue;

            var children = await db.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategoryId == current)
                .Select(c => c.Id)
                .ToListAsync();

            foreach (var child in children)
                queue.Enqueue(child);
        }

        return result;
    }

    // DTOs
    public record ProductListItem(int Id, string Name, decimal Price, int Stock, string CategoryName);
    public record ProductDetail(int Id, string Name, decimal Price, int Stock, CategoryInfo Category);
    public record CategoryInfo(int Id, string Name, CategoryInfo? Parent);
    public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);
}
