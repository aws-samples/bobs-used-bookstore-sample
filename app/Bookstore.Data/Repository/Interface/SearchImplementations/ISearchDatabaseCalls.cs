using System.Linq;

namespace Bookstore.Data.Repository.Interface.SearchImplementations
{
    public interface ISearchDatabaseCalls
    {
        IQueryable GetTable(string tableName);
    }
}