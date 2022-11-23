namespace Bookstore.Domain.Orders
{
    public enum OrderStatus
    {
        Pending = 0,
        Ordered = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }
}