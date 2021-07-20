using System.Linq;
using BobsBookstore.DataAccess.Data;
using BookstoreBackend.Repository.SearchImplementations;

namespace BookstoreBackend.Repository.Implementations.SearchImplementation
{
    public class SearchDatabaseCalls: ISearchDatabaseCalls
    {

        private ApplicationDbContext _context;

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
