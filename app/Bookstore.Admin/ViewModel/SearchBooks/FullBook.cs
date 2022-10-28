using Bookstore.Domain.Books;

namespace AdminSite.ViewModel
{
    public class FullBook
    {
        public int LowestPrice { get; set; }

        public int TotalQuantity { get; set; }

        public Price Price { get; set; }
    }
}