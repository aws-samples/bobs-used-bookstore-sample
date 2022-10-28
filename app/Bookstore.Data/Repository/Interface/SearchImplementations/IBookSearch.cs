using System.Collections.Generic;
using Bookstore.Domain.Books;

namespace Bookstore.Data.Repository.Interface.SearchImplementations
{
    public interface IBookSearch : IGenericRepository<Book>
    {
        public IEnumerable<Book> GetBooksbySearch(string searchString);
    }
}