using Bookstore.Data.Repository.Interface;
using Bookstore.Domain.Books;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public interface IInventoryService
    {
        Book GetBook(int id);

        IEnumerable<Book> GetBooks(string userName, int index, int count);

        (IEnumerable<Publisher> Publishers, IEnumerable<Genre> Genres, IEnumerable<Condition> Conditions, IEnumerable<BookType> BookTypes) GetReferenceData();

        Task SaveBookAsync(Book book);
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
            return bookRepository.Get2(x => x.Id == id, null, x => x.Genre, y => y.Publisher, x => x.Type).SingleOrDefault();
        }

        public IEnumerable<Book> GetBooks(string userName, int index, int count)
        {
            return bookRepository
                .Get2(x => x.CreatedBy == userName, y => y.OrderBy(x => x.CreatedOn), x => x.Genre, y => y.Publisher, x => x.Type)              
                .Skip(index).Take(count);
        }

        public (IEnumerable<Publisher> Publishers, 
                IEnumerable<Genre> Genres, 
                IEnumerable<Condition> Conditions, 
                IEnumerable<BookType> BookTypes) GetReferenceData()
        {
            throw new System.NotImplementedException();
        }

        public Task SaveBookAsync(Book book)
        {
            throw new System.NotImplementedException();
        }
    }
}
