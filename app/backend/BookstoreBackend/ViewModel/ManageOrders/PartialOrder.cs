using System.Collections.Generic;
using BobsBookstore.Models.Orders;

namespace BookstoreBackend.ViewModel.ManageOrders
{
    public class PartialOrderViewModel
    {
        public Order Order { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }

        public long OrderId { get; set; }

        public long OrderDetailId { get; set; }

        public string Quantity { get; set; }

        public bool IsLast { get; set; }

        public int MaxQuant { get; set; }

        public int ItemsRemoved { get; set; }
    }
}