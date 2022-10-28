using Bookstore.Domain.Orders;

namespace AdminSite.ViewModel.UpdateBooks
{
    public class FilterOrders
    {
        public Order Order { get; set; }
        public int Severity { get; set; }
    }
}