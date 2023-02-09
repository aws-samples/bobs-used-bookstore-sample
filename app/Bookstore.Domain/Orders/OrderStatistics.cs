namespace Bookstore.Domain.Orders
{
    public class OrderStatistics
    {
        public int PendingOrders { get; set; }

        public int PastDueOrders { get; set; }

        public int OrdersThisMonth { get; set; }

        public int OrdersTotal { get; set; }
    }
}