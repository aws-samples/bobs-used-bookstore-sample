namespace Bookstore.Domain.Orders
{
    public class OrderFilters
    {
        public OrderStatus? OrderStatusFilter { get; set; }

        public DateTime? OrderDateFromFilter { get; set; }

        public DateTime? OrderDateToFilter { get; set; }
    }
}