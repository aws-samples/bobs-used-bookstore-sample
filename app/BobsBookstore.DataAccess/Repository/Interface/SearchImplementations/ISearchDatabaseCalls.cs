using System.Linq;

namespace BobsBookstore.DataAccess.Repository.Interface.SearchImplementations
{
    public interface ISearchDatabaseCalls
    {
        IQueryable GetTable(string tableName);
    }
}
