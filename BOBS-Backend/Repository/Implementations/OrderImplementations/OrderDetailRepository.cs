using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BOBS_Backend.Repository.OrdersInterface;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;
using BOBS_Backend.Notifications.NotificationsInterface;

namespace BOBS_Backend.Repository.Implementations.OrderImplementations
{
    public class OrderDetailRepository : IOrderDetailRepository
    {

        /*
         * Repository with all the functions assoicated with the Order Detail Model
         * 
         */

        private DatabaseContext _context;
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderStatusRepository _orderStatusRepo;

        // Set up conncection to Database
        public OrderDetailRepository(DatabaseContext context, IOrderRepository ordeRepo, IOrderStatusRepository orderStatusRepo)
        {
            _context = context;
            _orderRepo = ordeRepo;
            _orderStatusRepo = orderStatusRepo;
        }


        public async Task<int> FindOrderDetailsRemovedCountAsync(long id)
        {
            var num = await _context.OrderDetail.Where(ord => ord.Order.Order_Id == id && ord.IsRemoved == true).CountAsync();

            return num;
        }

        public async Task<Order> CancelOrder(long id)
        {
            try
            {

                var orderStatus = await _orderStatusRepo.FindOrderStatusByName("Cancelled");

                var orderDetails = await FindOrderDetailByOrderId(id);
                var order = await _orderRepo.FindOrderById(id);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    foreach (var detail in orderDetails)
                    {
                        if (detail.IsRemoved != true)
                        {
                            order.Subtotal -= (detail.quantity * detail.price);
                            order.Tax -= (detail.quantity * detail.price * .1);

                            await _context.SaveChangesAsync();

                            detail.Price.Quantity += detail.quantity;
                            detail.IsRemoved = true;

                            await _context.SaveChangesAsync();

                        }
                    }

                    order.OrderStatus = orderStatus;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return order;
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public async Task<Dictionary<string,string>> MakeOrderDetailInactive(long id,long orderId,int quantity) 
        {
            try
            {
                var origOrderDetail = await FindOrderDetailById(id);

                var moneyOwe = origOrderDetail.price * origOrderDetail.quantity;


                var pending = await _orderStatusRepo.FindOrderStatusByName("Pending");

                var origOrder = await _orderRepo.FindOrderById(orderId);

                if (origOrderDetail.IsRemoved == true || origOrder.OrderStatus.position > pending.position) return null;

                using(var transaction = _context.Database.BeginTransaction())
                {

                    origOrder.Subtotal -= (moneyOwe);

                    origOrder.Tax -= (moneyOwe * .10);

                    await _context.SaveChangesAsync();

                    origOrderDetail.IsRemoved = true;

                    origOrderDetail.Price.Quantity += quantity;


                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    Dictionary<string, string> emailInfo = new Dictionary<string, string>
                {
                    { "bookName",origOrderDetail.Book.Name },
                    { "bookCondition",origOrderDetail.Price.Condition.ConditionName },
                    { "customerFirstName",origOrder.Customer.FirstName },
                    { "customerEmail",origOrder.Customer.Email }
                };
                    return emailInfo;
                }

               

            }
            catch(DbUpdateConcurrencyException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

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
