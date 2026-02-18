namespace EntretienCiteo.Api.Entities;

public class OrderLine
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    /// <summary>Price snapshotted at order creation time — does not change if product price changes later.</summary>
    public decimal UnitPrice { get; set; }
}
