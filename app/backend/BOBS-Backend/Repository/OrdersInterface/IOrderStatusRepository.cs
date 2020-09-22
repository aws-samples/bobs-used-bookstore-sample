using BOBS_Backend.Models.Order;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository
{
    public interface IOrderStatusRepository
    {
        /*
         *  Order Status Repository
         */
        Task<List<OrderStatus>> GetOrderStatuses();

        Task<OrderStatus> FindOrderStatusByName(string status);

        Task<OrderStatus> FindOrderStatusById(long id);

        Task<Order> UpdateOrderStatus(Order order, long Status_Id);

        IQueryable<OrderStatus> FilterOrderStatus(string filterValue, string searchString, string inBetween, string operand, string negate);
    }
}
