using Bookstore.Domain.Orders;

namespace Bookstore.Services.Filters
{
    public class OrderFilters
    {
        public OrderStatus? OrderStatusFilter { get; set; }
    }
}