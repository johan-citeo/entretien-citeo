namespace EntretienCiteo.Api.Entities;

public class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderLine> Lines { get; set; } = [];

    public decimal Total => Lines.Sum(l => l.UnitPrice * l.Quantity);
}
