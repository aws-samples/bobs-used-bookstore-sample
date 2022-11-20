using System.Collections.Generic;

namespace AdminSite.ViewModel.Inventory
{
    public abstract class PaginatedViewModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public List<int> PaginationButtons { get; set; } = new List<int>();
    }
}