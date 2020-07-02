using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.ViewModel.ManageOrders
{
    public class PartialOrder
    {
        /*
         * ViewModel PartialOrder
         * Stores One Order and the Order Details associated with it for use in the front end
         */

        public Order Order { get; set; }

        public IEnumerable<OrderDetail> OrderDetails {get; set;}
    }
}
