using System.Linq;
using BookstoreBackend.Database;
using BookstoreBackend.Repository.SearchImplementations;

namespace BookstoreBackend.Repository.Implementations.SearchImplementation
{
    public class SearchDatabaseCalls: ISearchDatabaseCalls
    {

        private DatabaseContext _context;

        public SearchDatabaseCalls(DatabaseContext context)
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
