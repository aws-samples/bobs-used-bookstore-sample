using System.Collections.Generic;
using BobsBookstore.Models.Books;

namespace BookstoreBackend.ViewModel.UpdateBooks
{
    public class LatestUpdates
    {
        public List<Price> UserBooks { get; set; }
        public List<Price> NotUserBooks { get; set; }

        public List<FilterOrders> ImpOrders { get; set; }
    }
}
