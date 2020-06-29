using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BOBS_Backend.Repository.Implementations
{
    public class OrderDetailRepository : IOrderDetailRepository
    {

        private DatabaseContext _context;

        public OrderDetailRepository(DatabaseContext context)
        {
            _context = context;
        }


        public async Task<OrderDetail> FindOrderDetailById(long id)
        {
            var orderDetail = await _context.OrderDetail
                                    .Where(x => x.OrderDetail_Id == id)
                                    .Include(o => o.Book)
                                    .Include(o => o.Price)
                                    .FirstAsync();

            return orderDetail;
        }


        public async Task<List<OrderDetail>> FindOrderDetailByOrderId(long orderId)
        {
            var orderDetails = await _context.OrderDetail
                                    .Where(x => x.Order.Order_Id == orderId)
                                    .Include(o => o.Book)
                                    .Include(o => o.Price)
                                    .ToListAsync();

            return orderDetails;
        }
    }
}
