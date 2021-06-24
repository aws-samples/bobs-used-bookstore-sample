using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreBackend.Repository.SearchImplementations
{
    public interface ISearchDatabaseCalls
    {
        IQueryable GetTable(string tableName);
    }
}
