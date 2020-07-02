using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.ViewModel.ManageOrders
{
    public class ManageOrderViewModel
    {
        /*
         * ManageOrder ViewModel
         * Contains the search query and the Table Column (FIlterValue) needed to fitler Orders
         */
        public string SearchString { get; set; }

        public string FilterValue { get; set; }
    }
}
