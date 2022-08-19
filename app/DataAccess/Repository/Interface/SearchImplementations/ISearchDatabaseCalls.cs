using System.Linq;

namespace DataAccess.Repository.Interface.SearchImplementations
{
    public interface ISearchDatabaseCalls
    {
        IQueryable GetTable(string tableName);
    }
}