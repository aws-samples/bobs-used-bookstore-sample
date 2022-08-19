using System.Collections.Generic;
using DataModels.Books;

namespace DataAccess.Repository.Interface.SearchImplementations
{
    public interface IBookSearch : IGenericRepository<Book>
    {
        public IEnumerable<Book> GetBooksbySearch(string searchString);
    }
}