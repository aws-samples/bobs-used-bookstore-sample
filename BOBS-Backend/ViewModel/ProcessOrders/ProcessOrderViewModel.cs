using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.ViewModel.ProcessOrders
{
    public class ProcessOrderViewModel
    {
        public long OrderId { get; set; }

        public long Status { get; set; }

    }
}
