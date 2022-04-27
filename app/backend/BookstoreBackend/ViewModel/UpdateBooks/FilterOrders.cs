using BobsBookstore.Models.Orders;

namespace BookstoreBackend.ViewModel.UpdateBooks
{
    public class FilterOrders
    {
        public Order Order { get; set; }
        public int Severity { get; set; }
    }
}