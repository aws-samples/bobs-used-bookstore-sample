using BOBS_Backend.Models.Order;
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

        public string FilterValueText { get; set; }

        public int PageNumber { get; set; }

        public List<Order> Orders { get; set; }

        public bool HasPreviousPages { get; set; }

        public int CurrentPage { get; set; }

        public bool HasNextPages { get; set; }

        public int[] Pages { get; set; }

        public string SortBy { get; set; }
    }
}
