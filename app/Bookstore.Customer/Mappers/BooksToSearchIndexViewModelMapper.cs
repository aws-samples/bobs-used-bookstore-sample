using Bookstore.Customer.ViewModel.Search;
using Bookstore.Domain;
using Bookstore.Domain.Books;
using System.Linq;

namespace Bookstore.Customer.Mappers
{
    public static class BooksToSearchIndexViewModelMapper
    {
        public static SearchIndexViewModel ToSearchIndexViewModel(this PaginatedList<Book> books)
        {
            var result = new SearchIndexViewModel();

            foreach (var book in books)
            {
                result.Books.Add(new SearchIndexItemViewModel
                {
                    BookId= book.Id,
                    BookName= book.Name,
                    ImageUrl = book.FrontImageUrl,
                    Price= book.Price,
                    Quantity= book.Quantity
                });
            }

            result.PageIndex = books.PageIndex;
            result.PageSize = books.Count;
            result.PageCount = books.TotalPages;
            result.HasNextPage = books.HasNextPage;
            result.HasPreviousPage = books.HasPreviousPage;
            result.PaginationButtons = books.GetPageList(5).ToList();

            return result;
        }
    }
}
