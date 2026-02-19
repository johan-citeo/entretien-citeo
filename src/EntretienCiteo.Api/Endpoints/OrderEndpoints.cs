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
    private static IResult CreateOrder(CreateOrderRequest request)
    {
        // valider stock avant de créer
        //Créer avec le statut Pending

        var orderId = 123;
        return Results.Created($"/orders/{orderId}", new OrderDetail(orderId, request.CustomerId, "", "", DateTime.Now, null, 1));
    }

    // GET /orders/{id}
    private static async Task<IResult> GetOrderById(int id)
    {
        //Renvoyer un OrderDetail
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
