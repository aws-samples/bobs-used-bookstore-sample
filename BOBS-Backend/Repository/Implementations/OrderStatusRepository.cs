using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.Implementations
{
    public class OrderStatusRepository : IOrderStatusRepository
    {

        private DatabaseContext _context;

        public OrderStatusRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<OrderStatus> FindOrderStatusById(long id)
        {
            var orderStatus = await _context.OrderStatus
                                       .Where(os => os.OrderStatus_Id == id)
                                       .FirstAsync();
            return orderStatus;
        }

        public async Task<List<OrderStatus>> GetOrderStatuses()
        {
            var orderStatus = await _context.OrderStatus.ToListAsync();
            return orderStatus;
        }

        public async Task<Order> UpdateOrderStatus(Order order, long Status_Id)
        {
            try
            {
                OrderStatus newStatus = await FindOrderStatusById(Status_Id);

                order.OrderStatus = newStatus;

                _context.Order.Update(order);
                await _context.SaveChangesAsync();

                return order;
            }
            catch (DbUpdateException)
            {

            }

            return order;
        }
    }
}
