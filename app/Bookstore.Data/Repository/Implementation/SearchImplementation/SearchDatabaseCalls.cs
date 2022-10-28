using System.Linq;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface.SearchImplementations;

namespace Bookstore.Data.Repository.Implementation.SearchImplementation
{
    public class SearchDatabaseCalls : ISearchDatabaseCalls
    {
        private readonly ApplicationDbContext _context;

        public SearchDatabaseCalls(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable GetTable(string tableName)
        {
            var result = (IQueryable)_context.GetType().GetProperty(tableName).GetValue(_context, null);
            return result;
        }
    }
}