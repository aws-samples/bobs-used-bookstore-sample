using System.Collections.Generic;
using BookstoreBackend.Models.Book;

namespace BookstoreBackend.ViewModel.UpdateBooks
{
    public class LatestUpdates
    {
        public List<Price> UserBooks { get; set; }
        public List<Price> NotUserBooks { get; set; }

        public List<FilterOrders> ImpOrders { get; set; }
    }
}
