using Bookstore.Domain.Books;
using Bookstore.Web.Areas.Admin.Models.Inventory;

namespace Bookstore.Web.Areas.Admin.Mappers.Inventory
{
    public static class BookToInventoryDetailsViewModelMapper
    {
        public static InventoryDetailsViewModel ToInventoryDetailsViewModel(this Book book)
        {
            var result = new InventoryDetailsViewModel
            {
                Author = book.Author,
                BookType = book.BookType.Text,
                Condition = book.Condition?.Text,
                CoverImageUrl= book.CoverImageUrl,
                Genre = book.Genre.Text,
                Id = book.Id,
                ISBN = book.ISBN,
                Name = book.Name,
                Price = book.Price,
                Publisher = book.Publisher.Text,
                Quantity = book.Quantity,
                Summary = book.Summary,
                Year = book.Year.GetValueOrDefault()
            };

            return result;
        }
    }
}
