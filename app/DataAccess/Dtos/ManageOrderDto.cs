using System.Collections.Generic;
using DataModels.Orders;

namespace DataAccess.Dtos
{
    public class ManageOrderDto
    {
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