using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using BookstoreBackend.Database;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Interface.OrdersInterface;

namespace BobsBookstore.DataAccess.Repository.Implementation.OrderImplementations
{
    public class OrderDatabaseCalls : IOrderDatabaseCalls
    {

        private ApplicationDbContext _context;

        public OrderDatabaseCalls(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable GetBaseQuery(string objPath)
        {
            var query = _context.Query(objPath);

            return query;
        }

        public IDbContextTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();

            return transaction;
        }

        public async Task TransactionCommitChanges(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public async Task ContextSaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> ReturnBaseQuery<T>(IQueryable query, string[] includes) where T : class
        {
            var result = (IQueryable<T>)query;
            result = result.Include(includes);

            return result;
        }

        public IQueryable<T> ReturnFilterQuery<T>(IQueryable<T> query, Expression<Func<T, bool>> lambda)
        {
            return query.Where(lambda);
        }
    }
}
