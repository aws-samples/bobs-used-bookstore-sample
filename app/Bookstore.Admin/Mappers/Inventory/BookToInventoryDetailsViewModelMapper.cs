using Bookstore.Admin.ViewModel.Inventory;
using Bookstore.Domain.Books;

namespace Bookstore.Admin.Mappers.Inventory
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

            if (!string.IsNullOrWhiteSpace(book.FrontImageUrl)) result.Images.Add(book.FrontImageUrl);
            if (!string.IsNullOrWhiteSpace(book.BackImageUrl)) result.Images.Add(book.BackImageUrl);
            if (!string.IsNullOrWhiteSpace(book.LeftImageUrl)) result.Images.Add(book.LeftImageUrl);
            if (!string.IsNullOrWhiteSpace(book.RightImageUrl)) result.Images.Add(book.RightImageUrl);

            return result;
        }
    }
}
