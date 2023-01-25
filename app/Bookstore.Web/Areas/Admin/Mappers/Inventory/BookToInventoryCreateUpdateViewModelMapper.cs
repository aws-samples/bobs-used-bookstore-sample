using Bookstore.Domain.Books;
using Bookstore.Web.Areas.Admin.Models.Inventory;

namespace Bookstore.Web.Areas.Admin.Mappers.Inventory
{
    public static class BookToInventoryCreateUpdateViewModelMapper
    {
        public static InventoryCreateUpdateViewModel ToInventoryCreateUpdateViewModel(this Book book)
        {
            var result = new InventoryCreateUpdateViewModel
            {
                Author = book.Author,
                BackImageUrl = book.BackImageUrl,
                FrontImageUrl = book.FrontImageUrl,
                Id = book.Id,
                ISBN = book.ISBN,
                LeftImageUrl = book.LeftImageUrl,
                Name = book.Name,
                Price = book.Price,
                Quantity = book.Quantity,
                RightImageUrl = book.RightImageUrl,
                SelectedBookTypeId = book.BookType.Id,
                SelectedConditionId = book.Condition.Id,
                SelectedGenreId = book.Genre.Id,
                SelectedPublisherId = book.Publisher.Id,
                Summary = book.Summary,
                Year = book.Year.GetValueOrDefault()
            };

            return result;
        }
    }
}