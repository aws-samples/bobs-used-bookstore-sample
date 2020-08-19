using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository.OrdersInterface;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.Implementations.OrderImplementations
{
    public class OrderDatabaseCalls : IOrderDatabaseCalls
    {

        private DatabaseContext _context;

        public OrderDatabaseCalls(DatabaseContext context)
        {
            _context = context;
        }

        public IQueryable GetBaseQuery(string objPath)
        {
            var query = _context.Query(objPath);

            return query;
        }

        public IQueryable<Order> ReturnBaseOrderQuery(IQueryable query, string[] includes)
        {
            var result = (IQueryable<Order>)query;
            result = result.Include(includes);

            return result;
        }

        public IQueryable<Order> ReturnFilterOrderQuery(IQueryable<Order> query,Expression<Func<Order,bool>> lambda)
        {
            return query.Where(lambda);
        }

        public IQueryable<OrderStatus> ReturnBaseOrderStatusQuery(IQueryable query, string[] includes)
        {
            var result = (IQueryable<OrderStatus>)query;
            result = result.Include(includes);

            return result;
        }

        public IQueryable<OrderStatus> ReturnFilterOrderStatusQuery(IQueryable<OrderStatus> query, Expression<Func<OrderStatus, bool>> lambda)
        {
            return query.Where(lambda);
        }

        public IQueryable<OrderDetail> ReturnBaseOrderDetailQuery(IQueryable query, string[] includes)
        {
            var result = (IQueryable<OrderDetail>)query;
            result = result.Include(includes);

            return result;
        }

        public IQueryable<OrderDetail> ReturnFilterOrderDetailQuery(IQueryable<OrderDetail> query, Expression<Func<OrderDetail, bool>> lambda)
        {
            return query.Where(lambda);
        }

    }
}
