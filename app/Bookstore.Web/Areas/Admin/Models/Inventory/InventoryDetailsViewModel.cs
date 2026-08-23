using Bookstore.Domain.Books;

namespace Bookstore.Web.Areas.Admin.Models.Inventory
{
    public class InventoryDetailsViewModel
    {
        public InventoryDetailsViewModel() { }

        public InventoryDetailsViewModel(Book book)
        {
            Author = book.Author;
            BookType = book.BookType.Text;
            Condition = book.Condition.Text;
            CoverImageUrl = book.CoverImageUrl;
            Genre = book.Genre.Text;
            Id = book.Id;
            ISBN = book.ISBN;
            Name = book.Name;
            Price = book.Price;
            Publisher = book.Publisher.Text;
            Quantity = book.Quantity;
            Summary = book.Summary;
            Year = book.Year.GetValueOrDefault();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int Year { get; set; }

        public string Publisher { get; set; } = null!;

        public string BookType { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public string Condition { get; set; } = null!;

        public string? CoverImageUrl { get; set; }

        public string? Summary { get; set; }

        public string ISBN { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
