using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface.OrdersInterface;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bookstore.Data.Repository.Implementation.OrderImplementations
{
    public class OrderDatabaseCalls : IOrderDatabaseCalls
    {
        private readonly ApplicationDbContext _context;

        public OrderDatabaseCalls(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable GetBaseQuery(string objPath)
        {
            return _context.Query(objPath);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task TransactionCommitChangesAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public async Task ContextSaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> ReturnBaseQuery<T>(IQueryable query, string[] includes) where T : class
        {
            var result = (IQueryable<T>)query;
            return result.Include(includes);
        }

        public IQueryable<T> ReturnFilterQuery<T>(IQueryable<T> query, Expression<Func<T, bool>> lambda)
        {
            return query.Where(lambda);
        }
    }
}
