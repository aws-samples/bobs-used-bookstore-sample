using System.Collections.Generic;

namespace BookstoreBackend.ViewModel.SearchBooks
{
    public class SearchBookViewModel
    {
        public string SearchBy { get; set; }

        public string SearchFilter { get; set; }

        public int PageNumber { get; set; }

        public List<FullBook> Books { get; set; }

        public bool HasPreviousPages { get; set; }

        public int CurrentPage { get; set; }

        public bool HasNextPages { get; set; }

        public int[] Pages { get; set; }

        public string ViewStyle { get; set; }

        public string SortBy { get; set; }

        public string Ascdesc { get; set; }
    }
}