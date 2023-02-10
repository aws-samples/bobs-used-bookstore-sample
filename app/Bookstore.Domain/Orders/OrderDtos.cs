namespace Bookstore.Domain.Orders
{
    public record CreateOrderDto(string CustomerSub, string CorrelationId, int AddressId);

    public record UpdateOrderStatusDto(int OrderId, OrderStatus OrderStatus);

    public record CancelOrderDto(string CustomerSub, int OrderId);
}