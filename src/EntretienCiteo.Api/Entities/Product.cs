namespace EntretienCiteo.Api.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public List<OrderLine> OrderLines { get; set; } = [];

    /// <summary>Incremented on every update — used as optimistic concurrency token.</summary>
    public Guid RowVersion { get; set; } = Guid.NewGuid();
}
