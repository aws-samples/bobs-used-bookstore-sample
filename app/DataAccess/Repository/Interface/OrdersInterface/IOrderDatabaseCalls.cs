using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repository.Interface.OrdersInterface
{
    public interface IOrderDatabaseCalls
    {
        IQueryable GetBaseQuery(string objPath);

        IDbContextTransaction BeginTransaction();

        Task TransactionCommitChanges(IDbContextTransaction transaction);

        Task ContextSaveChanges();

        IQueryable<T> ReturnBaseQuery<T>(IQueryable query, string[] includes) where T : class;

        IQueryable<T> ReturnFilterQuery<T>(IQueryable<T> query, Expression<Func<T, bool>> lambda);
    }
}