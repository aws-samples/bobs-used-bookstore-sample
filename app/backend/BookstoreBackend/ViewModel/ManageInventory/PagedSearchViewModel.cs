using System.Collections.Generic;

namespace BookstoreBackend.ViewModel.ManageInventory
{
    public class PagedSearchViewModel
    {
        public string Searchby { get; set; }

        public string Searchfilter { get; set; }

        public int PageNumber { get; set; }

        public IEnumerable<BookDetailsViewModel> Books { get; set; }

        public bool HasPreviousPages { get; set; }

        public int CurrentPage { get; set; }

        public bool HasNextPages { get; set; }

        public int[] Pages { get; set; }

        public string ViewStyle { get; set; }

        public string SortBy { get; set; }

        public string Ascdesc { get; set; }

        public string Pagination { get; set; }
    }
}