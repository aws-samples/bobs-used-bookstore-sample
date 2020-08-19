using BOBS_Backend.Database;
using BOBS_Backend.Repository.SearchImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.Implementations.SearchImplementation
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
