using BookstoreBackend.Database;
using BobsBookstore.Models.Orders;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BobsBookstore.DataAccess.Repository.Interface.OrdersInterface
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
