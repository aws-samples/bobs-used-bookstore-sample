using BobsBookstore.Models.Orders;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobsBookstore.DataAccess.Repository.Interface.OrdersInterface
{
    public interface IOrderStatusRepository
    {
        /*
         *  Order Status Repository
         */
        public List<OrderStatus> GetOrderStatuses();

        public OrderStatus FindOrderStatusByName(string status);

        public OrderStatus FindOrderStatusById(long id);

        Task<Order> UpdateOrderStatus(Order order, long Status_Id);

        IQueryable<OrderStatus> FilterOrderStatus(string filterValue, string searchString, string inBetween, string operand, string negate);
    }
}
