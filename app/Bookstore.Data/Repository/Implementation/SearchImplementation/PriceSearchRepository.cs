using System.Collections.Generic;
using System.Linq;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Implementation;
using Bookstore.Data.Repository.Interface.SearchImplementations;
using Bookstore.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Repository.Implementation.SearchImplementation
{
    public class PriceSearchRepository : GenericRepository<Price>, IPriceSearch
    {
        private readonly ApplicationDbContext _context;

        public PriceSearchRepository(ApplicationDbContext db) : base(db)
        {
            _context = db;
        }

        public IEnumerable<Price> GetPricebySearch(string searchString)
        {
            IEnumerable<Price> price = _context.Price.Where(p => p.Quantity > 0 &&
                                                                 p.Active && (
                                                                     p.Book.Name.Contains(searchString) ||
                                                                     p.Book.Genre.Text.Contains(searchString) ||
                                                                     p.Book.BookType.Text.Contains(searchString) ||
                                                                     p.Book.ISBN.Contains(searchString) ||
                                                                     p.Book.Publisher.Text.Contains(searchString)))
                .Include(price => price.Book)
                .ThenInclude(book => book.Genre)
                .Include(price => price.Book)
                .ThenInclude(book => book.BookType)
                .Include(price => price.Book)
                .ThenInclude(book => book.Publisher);

            return price;
        }
    }
}