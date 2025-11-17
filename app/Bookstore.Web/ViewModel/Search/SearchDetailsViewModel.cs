using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Bookstore.Domain.Books;

namespace Bookstore.Web.ViewModel.Search
{
    public class SearchDetailsViewModel
    {
        public int BookId { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        [Display(Name = "Title")]
        [DefaultValue("Title")]
        public string BookName { get; set; }

        [DefaultValue("Publisher not found")] public string PublisherName { get; set; }

        [DefaultValue("No Author")] public string Author { get; set; }

        public string ISBN { get; set; }

        [Display(Name = "Genre")] public string GenreName { get; set; }

        [Display(Name = "Type")] public string TypeName { get; set; }

        [Display(Name = "Condition")] public string ConditionName { get; set; }

        public string Url { get; set; }

        [Display(Name = "$$")] public decimal MinPrice { get; set; }

        public int Quantity { get; set; }

        public string Summary { get; set; }

        public SearchDetailsViewModel(Book book)
        {
            BookName = book.Name;
            Author = book.Author;
            PublisherName = book.Publisher.Text;
            ISBN = book.ISBN;
            GenreName = book.Genre.Text;
            TypeName = book.BookType.Text;
            ConditionName = book.Condition.Text;
            Url = book.CoverImageUrl;
            MinPrice = book.Price;
            Quantity = book.Quantity;
            BookId = book.Id;
            Summary = book.Summary;
        }
    }
}
