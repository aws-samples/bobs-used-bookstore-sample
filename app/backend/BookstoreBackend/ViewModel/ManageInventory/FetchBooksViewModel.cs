using BookstoreBackend.ViewModel.ManageInventory;
using System.Collections.Generic;

namespace BookstoreBackend.ViewModel
{
    public class FetchBooksViewModel
    {
        public string BookName { get; set; }
        public string Publisher { get; set; }
        public string BookType { get; set; }
        public string Genre { get; set; }
        public string Condition { get; set; }

        public IEnumerable<BookDetailsViewModel> Books { get; set; }

        public string SearchFilter { get; set; }

        public string SearchBy { get; set; }

        public string FrontUrl { get; set; }

        public string BackUrl { get; set; }

        public string LeftUrl { get; set; }

        public string RightUrl { get; set; }

        public string TypeChosen { get; set; }

        public string ViewStyle { get; set; }

        public string Type {get;set;}

        public string ConditionChosen { get; set; }


        public string Author { get; set; }

        public string Summary { get; set; }

        public string ISBN { get; set; }

    }
}
