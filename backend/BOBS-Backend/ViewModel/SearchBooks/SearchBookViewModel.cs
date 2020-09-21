using BOBS_Backend.Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.ViewModel.SearchBooks
{
    public class SearchBookViewModel
    {
        public string searchby { get; set; }

        public string searchfilter { get; set; }

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
