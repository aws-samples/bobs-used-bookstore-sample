using AutoMapper;
using Bookstore.Data.Repository.Interface;
using Bookstore.Domain.Books;
using Services.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public interface IInventoryService
    {
        Book GetBook(int id);

        IEnumerable<BookDto> GetBooks(string userName, int index, int count);

        Task SaveBookAsync(Book book);
    }

    public class InventoryService : IInventoryService
    {
        private readonly IGenericRepository<Book> bookRepository;
        private readonly IMapper mapper;

        public InventoryService(IGenericRepository<Book> bookRepository, IMapper mapper)
        {
            this.bookRepository = bookRepository;
            this.mapper = mapper;
        }

        public Book GetBook(int id)
        {
            return bookRepository.Get(id);
        }

        public IEnumerable<BookDto> GetBooks(string userName, int index, int count)
        {
            var books = bookRepository.Get(x => x.CreatedBy == userName).Skip(index).Take(count);

            return mapper.Map<IEnumerable<BookDto>>(books);
        }

        public Task SaveBookAsync(Book book)
        {
            throw new System.NotImplementedException();
        }
    }
}
