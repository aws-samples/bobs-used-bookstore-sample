using BOBS_Backend.Models.Order;
using BOBS_Backend.ViewModel.ManageOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.ViewModel.ProcessOrders
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

        public PartialOrder FullOrder { get; set; }

        public string errorMessage { get; set; }

    }
}
