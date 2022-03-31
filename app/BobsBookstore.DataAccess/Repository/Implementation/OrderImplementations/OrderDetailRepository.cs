using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BobsBookstore.Models.Orders;
using BobsBookstore.DataAccess.Repository.Interface.OrdersInterface;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;

namespace BobsBookstore.DataAccess.Repository.Implementation.OrderImplementations
{
    // Repository with all the functions assoicated with the Order Detail Model
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly string[] OrderDetailIncludes = { "Book", "Book.Genre", "Book.Publisher","Book.Type","Price","Price.Condition" };
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderStatusRepository _orderStatusRepo;
        private IOrderDatabaseCalls _orderDbCalls;
        private IExpressionFunction _expFunc;
        private readonly ILogger<OrderDetailRepository> _logger;

        public OrderDetailRepository(ILogger<OrderDetailRepository> logger, 
                                     IOrderRepository ordeRepo, 
                                     IOrderStatusRepository orderStatusRepo,
                                     IOrderDatabaseCalls orderDbCalls, 
                                     IExpressionFunction expFunc)
        {
            _orderRepo = ordeRepo;
            _orderStatusRepo = orderStatusRepo;
            _orderDbCalls = orderDbCalls;
            _expFunc = expFunc;
            _logger = logger;
        }

        public async Task<int> FindOrderDetailsRemovedCountAsync(long id)
        {
            string filterValue = "Order.Order_Id IsRemoved";
            string searchString = "" + id + "&&true";
            string inBetween = "And";
            string operand = "== ==";
            string negate = "false false";

            var query = FilterOrderDetail(filterValue, searchString, inBetween, operand, negate);

            var num = query.Count();

            return num;
        }

        public async Task<Order> CancelOrder(long id)
        {
            try
            {
                var orderStatus = _orderStatusRepo.FindOrderStatusByName("Cancelled");

                var orderDetails = await FindOrderDetailByOrderId(id);
                var order = _orderRepo.FindOrderById(id);

                using (var transaction = _orderDbCalls.BeginTransaction())
                {
                    foreach (var detail in orderDetails)
                    {
                        if (detail.IsRemoved != true)
                        {
                            order.Subtotal -= (detail.Quantity * detail.OrderDetailPrice);
                            order.Tax -= detail.Quantity * detail.OrderDetailPrice * (decimal).1;

                            await _orderDbCalls.ContextSaveChanges();

                            detail.Price.Quantity += detail.Quantity;
                            detail.IsRemoved = true;

                            await _orderDbCalls.ContextSaveChanges();
                        }
                    }

                    order.OrderStatus = orderStatus;

                    await _orderDbCalls.ContextSaveChanges();

                    await _orderDbCalls.TransactionCommitChanges(transaction);

                    return order;
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "DBConcurrency Error");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return null;
            }
        }
        
        public async Task<Dictionary<string,string>> MakeOrderDetailInactive(long id, long orderId, int quantity) 
        {
            try
            {
                var origOrderDetail = await FindOrderDetailById(id);

                var moneyOwe = origOrderDetail.OrderDetailPrice * origOrderDetail.Quantity;

                var pending = _orderStatusRepo.FindOrderStatusByName("Pending");

                var origOrder = _orderRepo.FindOrderById(orderId);

                if (origOrderDetail.IsRemoved == true || origOrder.OrderStatus.Position > pending.Position) return null;

                using(var transaction = _orderDbCalls.BeginTransaction())
                {

                    origOrder.Subtotal -= (moneyOwe);

                    origOrder.Tax -= (moneyOwe * (decimal).10);

                    await _orderDbCalls.ContextSaveChanges();

                    origOrderDetail.IsRemoved = true;

                    origOrderDetail.Price.Quantity += quantity;


                    await _orderDbCalls.ContextSaveChanges();

                    await _orderDbCalls.TransactionCommitChanges(transaction);
                    var emailInfo = new Dictionary<string, string>
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
                _logger.LogError(ex, "DBConcurrency Error");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return null;
            }

        }

        // Finds one instance of Order Detail Model by Order Detail Id
        public async Task<OrderDetail> FindOrderDetailById(long id)
        {
            string filterValue = "OrderDetail_Id";
            string searchString = "" + id;
            string inBetween = "";
            string operand = "==";
            string negate = "false";

            var query = FilterOrderDetail(filterValue, searchString, inBetween, operand, negate);

            var orderDetail = query.First();

            return orderDetail;
        }

        // Finds a List of Order Details by the assoicated Order Id
        public async Task<List<OrderDetail>> FindOrderDetailByOrderId(long orderId)
        {
            string filterValue = "Order.Order_Id";
            string searchString = "" + orderId;
            string inBetween = "";
            string operand = "==";
            string negate = "false";

            var query = FilterOrderDetail(filterValue, searchString, inBetween, operand, negate);

            var orderDetail = query.ToList();

            return orderDetail;
        }

        public IQueryable<OrderDetail> FilterOrderDetail(string filterValue, string searchString, string inBetween, string operand, string negate)
        {
            string tableName = "OrderDetail";

            var lambda = _expFunc.ReturnLambdaExpression<OrderDetail>(tableName, filterValue, searchString, inBetween, operand, negate);

            var orderDetailBase = _orderDbCalls.GetBaseQuery("BobsBookstore.Models.Orders.OrderDetail");

            var query = _orderDbCalls.ReturnBaseQuery<OrderDetail>(orderDetailBase, OrderDetailIncludes);

            var filterQuery = _orderDbCalls.ReturnFilterQuery(query, lambda);

            return filterQuery;
        }
    }
}
