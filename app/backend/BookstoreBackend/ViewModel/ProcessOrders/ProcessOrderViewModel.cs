using System.Collections.Generic;
using BobsBookstore.Models.Orders;
using BookstoreBackend.ViewModel.ManageOrders;

namespace BookstoreBackend.ViewModel.ProcessOrders
{
    public class ProcessOrderViewModel
    {
        /*
         * ProccssOrder ViewModel
         * Containes Order Id and New Status Id to Update Order Status
         */
        public long OrderId { get; set; }

        public long Status { get; set; }

        public long oldStatus { get; set; }
        
        public Order order { get; set; }

        public List<OrderStatus> Statuses {get; set;}

        public PartialOrderViewModel FullOrder { get; set; }

        public string errorMessage { get; set; }

    }
}
