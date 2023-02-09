namespace Bookstore.Domain.Orders
{
    public record UpdateOrderStatusDto(int OrderId, OrderStatus OrderStatus);
}