using Bookstore.Admin.ViewModel.Inventory;
using Bookstore.Domain.Books;

namespace Bookstore.Admin.Mappers.Inventory
{
    public static class InventoryCreateUpdateViewModelToBookMapper
    {
        public static Book ToBook(this InventoryCreateUpdateViewModel viewModel)
        {
            var book = new Book();

            return ToBook(viewModel, book);
        }

        public static Book ToBook(this InventoryCreateUpdateViewModel viewModel, Book book)
        {
            book.PublisherId = viewModel.SelectedPublisherId;
            book.Author = viewModel.Author;
            book.BookTypeId = viewModel.SelectedBookTypeId;
            book.ConditionId = viewModel.SelectedConditionId;
            book.GenreId = viewModel.SelectedGenreId;
            book.Id = viewModel.Id;
            book.ISBN = viewModel.ISBN;
            book.Name = viewModel.Name;
            book.Price = viewModel.Price;
            book.PublisherId = viewModel.SelectedPublisherId;
            book.Quantity = viewModel.Quantity;
            book.Summary = viewModel.Summary;
            book.Year = viewModel.Year;

            return book;
        }
    }
}
