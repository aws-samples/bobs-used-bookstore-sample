using Bookstore.Customer.ViewModel.Search;
using Bookstore.Domain.Books;

namespace Bookstore.Customer.Mappers
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
                Url = book.FrontImageUrl,
                MinPrice = book.Price,
                Quantity = book.Quantity,
                BookId = book.Id,
                Summary = book.Summary
            };
        }
    }
}