using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BOBS_Backend.Repository.OrdersInterface;

namespace BOBS_Backend.Repository.Implementations.OrderImplementations
{
    public class OrderDetailRepository : IOrderDetailRepository
    {

        /*
         * Repository with all the functions assoicated with the Order Detail Model
         * 
         */

        private DatabaseContext _context;

        // Set up conncection to Database
        public OrderDetailRepository(DatabaseContext context)
        {
            _context = context;
        }

        // Finds One instance of Order Detail Model by Order Detail Id
        public async Task<OrderDetail> FindOrderDetailById(long id)
        {
            var orderDetail = await _context.OrderDetail
                                    .Where(x => x.OrderDetail_Id == id)
                                    .Include(o => o.Book)
                                        .ThenInclude(b => b.Genre)
                                    .Include(o => o.Book)
                                        .ThenInclude(b => b.Publisher)
                                    .Include(o => o.Book)
                                        .ThenInclude(b => b.Type)
                                    .Include(o => o.Price)
                                        .ThenInclude(p => p.Condition)
                                    .FirstAsync();

            return orderDetail;
        }

        // Finds a List of Order Details by the assoicated Order Id
        public async Task<List<OrderDetail>> FindOrderDetailByOrderId(long orderId)
        {
            var orderDetails = await _context.OrderDetail
                                    .Where(x => x.Order.Order_Id == orderId)
                                    .Include(o => o.Book)
                                        .ThenInclude(b => b.Genre)
                                    .Include(o => o.Book)
                                        .ThenInclude(b => b.Publisher)
                                    .Include(o => o.Book)
                                        .ThenInclude(b => b.Type)
                                    .Include(o => o.Price)
                                        .ThenInclude(p => p.Condition)
                                    .ToListAsync();

            return orderDetails;
        }
    }
}
