using BookstoreBackend.ViewModel.ManageInventory;
using System.Collections.Generic;

namespace BookstoreBackend.ViewModel
{
    public class FetchBooksViewModel
    {
        public string BookName { get; set; }
        public string publisher { get; set; }
        public string BookType { get; set; }
        public string genre { get; set; }
        public string Condition { get; set; }

        public IEnumerable<BookDetailsViewModel> Books { get; set; }

        public string searchfilter { get; set; }

        public string searchby { get; set; }

        public string front_url { get; set; }

        public string back_url { get; set; }

        public string left_url { get; set; }

        public string right_url { get; set; }

        public string typechosen { get; set; }

        public string ViewStyle { get; set; }

        public string type {get;set;}

        public string condition_chosen { get; set; }


        public string Author { get; set; }

        public string Summary { get; set; }

        public string ISBN { get; set; }

    }
}
