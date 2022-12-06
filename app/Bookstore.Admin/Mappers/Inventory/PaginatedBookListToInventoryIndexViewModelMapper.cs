using Bookstore.Admin.ViewModel.Inventory;
using Bookstore.Domain;
using Bookstore.Domain.Books;
using System.Linq;

namespace Bookstore.Admin.Mappers.Inventory
{
    public static class PaginatedOrderListToOrderIndexViewModelMapper
    {
        public static InventoryIndexViewModel ToInventoryIndexViewModel(this PaginatedList<Book> books)
        {
            var result = new InventoryIndexViewModel();

            foreach (var book in books)
            {
                result.Items.Add(new InventoryIndexListItemViewModel
                {
                    Id = book.Id,
                    Name = book.Name,
                    Author = book.Author,
                    BookType = book.BookType.Text,
                    Condition = book.Condition?.Text,
                    Genre = book.Genre.Text,
                    Publisher = book.Publisher.Text,
                    UpdatedOn = book.UpdatedOn,
                    Year = book.Year.GetValueOrDefault(),
                    Price = book.Price,
                    Quantity = book.Quantity
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
