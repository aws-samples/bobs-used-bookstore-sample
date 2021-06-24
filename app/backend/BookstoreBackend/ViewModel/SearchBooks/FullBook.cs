using BookstoreBackend.Models.Book;

namespace BookstoreBackend.ViewModel.SearchBooks
{
    public class FullBook
    {

        public int LowestPrice { get; set; }

        public int TotalQuantity { get; set; }

        public Price Price { get; set; }
    }
}
