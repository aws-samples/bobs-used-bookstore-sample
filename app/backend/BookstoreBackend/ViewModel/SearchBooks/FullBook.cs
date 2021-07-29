using BobsBookstore.Models.Books;

namespace BookstoreBackend.ViewModel
{
    public class FullBook
    {

        public int LowestPrice { get; set; }

        public int TotalQuantity { get; set; }

        public Price Price { get; set; }
    }
}
