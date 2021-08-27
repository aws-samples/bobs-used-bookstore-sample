using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;
using BobsBookstore.Models.Books;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobsBookstore.DataAccess.Repository.Implementation.SearchImplementation
{
    public class BookSearchRepository : GenericRepository<Book>, IBookSearch
    {
        private ApplicationDbContext _context;

        public BookSearchRepository(ApplicationDbContext db) : base(db)
        {
            _context = db;
        }

        public IEnumerable<Book> GetBooksbySearch(string searchString)
        {
            return _context.Book.Where(b => b.Name.Contains(searchString) ||
                            b.Genre.Name.Contains(searchString) ||
                            b.Type.TypeName.Contains(searchString) ||
                            b.ISBN.Contains(searchString) ||
                            b.Publisher.Name.Contains(searchString))
                            .Include(book => book.Genre)
                            .Include(book => book.Type)
                            .Include(book => book.Publisher);
            
        }
    }
}
