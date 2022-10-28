using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bookstore.Data.Repository.Interface.OrdersInterface
{
    public interface IOrderDatabaseCalls
    {
        IQueryable GetBaseQuery(string objPath);

        IDbContextTransaction BeginTransaction();

        Task TransactionCommitChangesAsync(IDbContextTransaction transaction);

        Task ContextSaveChangesAsync();

        IQueryable<T> ReturnBaseQuery<T>(IQueryable query, string[] includes) where T : class;

        IQueryable<T> ReturnFilterQuery<T>(IQueryable<T> query, Expression<Func<T, bool>> lambda);
    }
}