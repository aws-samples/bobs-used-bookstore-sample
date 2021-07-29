using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobsBookstore.DataAccess.Repository.Interface.SearchImplementations
{
    public interface ISearchDatabaseCalls
    {
        IQueryable GetTable(string tableName);
    }
}
