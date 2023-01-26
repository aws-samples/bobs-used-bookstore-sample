using Bookstore.Domain.Books;
using Bookstore.Web.ViewModel.Search;

namespace Bookstore.Web.Mappers
{
    public static class BookToSearchDetailsViewModelMapper
    {
        public static SearchDetailsViewModel ToSearchDetailsViewModel(this Book book)
        {
            return new SearchDetailsViewModel
            {
                BookName = book.Name,
                Author = book.Author,
                PublisherName = book.Publisher.Text,
                ISBN = book.ISBN,
                GenreName = book.Genre.Text,
                TypeName = book.BookType.Text,
                ConditionName = book.Condition.Text,
                Url = book.CoverImageUrl,
                MinPrice = book.Price,
                Quantity = book.Quantity,
                BookId = book.Id,
                Summary = book.Summary
            };
        }
    }
}