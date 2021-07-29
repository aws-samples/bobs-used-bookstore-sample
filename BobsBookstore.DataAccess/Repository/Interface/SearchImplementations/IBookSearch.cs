using BobsBookstore.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobsBookstore.DataAccess.Repository.Interface.SearchImplementations
{
    public interface IBookSearch : IGenericRepository<Book>
    {
        public IEnumerable<Book> GetBooksbySearch(string searchString);
    }
    
}
