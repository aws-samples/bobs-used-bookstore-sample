using System.Collections.Generic;
using BobsBookstore.Models.Books;

namespace BobsBookstore.DataAccess.Repository.Interface.SearchImplementations
{
    public interface IBookSearch : IGenericRepository<Book>
    {
        public IEnumerable<Book> GetBooksbySearch(string searchString);
    }
}
