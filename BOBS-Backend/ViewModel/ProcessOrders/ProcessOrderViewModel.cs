using BOBS_Backend.Models.Order;
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

    }
}
