using Bookstore.Data.Repository.Interface;
using Bookstore.Domain.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public interface IInventoryService
    {
        Book GetBook(int id);

        IEnumerable<Book> GetBooks(string userName, int index, int count);

        Task SaveBookAsync(Book book, string userName);
    }

    public class InventoryService : IInventoryService
    {
        private readonly IGenericRepository<Book> bookRepository;

        public InventoryService(IGenericRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public Book GetBook(int id)
        {
            return bookRepository.Get2(x => x.Id == id, null, x => x.Genre, y => y.Publisher, x => x.BookType).SingleOrDefault();
        }

        public IEnumerable<Book> GetBooks(string userName, int index, int count)
        {
            return bookRepository
                .Get2(x => x.CreatedBy == userName, y => y.OrderBy(x => x.CreatedOn), x => x.Genre, y => y.Publisher, x => x.BookType)
                .Skip(index).Take(count);
        }

        public async Task SaveBookAsync(Book book, string userName)
        {
            if (book.IsNewEntity()) book.CreatedBy = userName;

            book.UpdatedOn = DateTime.UtcNow;

            bookRepository.AddOrUpdate(book);

            await bookRepository.SaveAsync();
        }
    }
}
