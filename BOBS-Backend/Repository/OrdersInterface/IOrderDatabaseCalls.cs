using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.OrdersInterface
{
    public interface IOrderDatabaseCalls
    {

        IQueryable GetBaseQuery(string objPath);

        IQueryable<Order> ReturnBaseOrderQuery(IQueryable query, string[] includes);


        IQueryable<Order> ReturnFilterOrderQuery(IQueryable<Order> query, Expression<Func<Order, bool>> lambda);


        IQueryable<OrderStatus> ReturnBaseOrderStatusQuery(IQueryable query, string[] includes);


        IQueryable<OrderStatus> ReturnFilterOrderStatusQuery(IQueryable<OrderStatus> query, Expression<Func<OrderStatus, bool>> lambda);


        IQueryable<OrderDetail> ReturnBaseOrderDetailQuery(IQueryable query, string[] includes);


        IQueryable<OrderDetail> ReturnFilterOrderDetailQuery(IQueryable<OrderDetail> query, Expression<Func<OrderDetail, bool>> lambda);

    }
}
