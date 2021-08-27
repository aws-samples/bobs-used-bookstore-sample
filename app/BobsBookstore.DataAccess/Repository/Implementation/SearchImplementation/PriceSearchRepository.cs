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
    public class PriceSearchRepository : GenericRepository<Price>, IPriceSearch
    {
        private ApplicationDbContext _context;

        public PriceSearchRepository(ApplicationDbContext db) : base(db)
        {
            _context = db;
        }

        public IEnumerable<Price> GetPricebySearch(string searchString)
        {
            IEnumerable<Price> price = _context.Price.Where(p => (p.Quantity > 0 &&
                             p.Active) && (
                            p.Book.Name.Contains(searchString) ||
                            p.Book.Genre.Name.Contains(searchString) ||
                            p.Book.Type.TypeName.Contains(searchString) ||
                            p.Book.ISBN.Contains(searchString) ||
                            p.Book.Publisher.Name.Contains(searchString)))
                 .Include(price => price.Book)
                 .ThenInclude(book => book.Genre)
                 .Include(price => price.Book)
                 .ThenInclude(book => book.Type)
                 .Include(price => price.Book)
                 .ThenInclude(book => book.Publisher);
            
                return price;
                
        }
    }
}
