using System.Collections.Generic;
using BobsBookstore.Models.Orders;

namespace BookstoreBackend.ViewModel.ManageOrders
{
    public class PartialOrder
    {
        /*
         * ViewModel PartialOrder
         * Stores One Order and the Order Details associated with it for use in the front end
         */

        public Order Order { get; set; }

        public IEnumerable<OrderDetail> OrderDetails {get; set;}
        
        public long orderId { get; set; }
        public long orderDetailId { get; set; }

        public string quantity { get; set; }

        public bool isLast { get; set; }
       
        public int maxQuant { get; set; }

        public int itemsRemoved { get; set; }

    }
}
