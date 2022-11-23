using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Data.Repository.Interface.OrdersInterface;
using Bookstore.Data.Repository.Interface.SearchImplementations;
using Bookstore.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookstore.Data.Repository.Implementation.OrderImplementations
{
    // Repository with all the functions assoicated with the Order Detail Model
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ILogger<OrderDetailRepository> _logger;
        private readonly IOrderRepository _orderRepo;

        private readonly string[] OrderDetailIncludes =
            { "Book", "Book.Genre", "Book.Publisher", "Book.Type", "Price", "Price.Condition" };

        private readonly IExpressionFunction _expFunc;
        private readonly IOrderDatabaseCalls _orderDbCalls;

        public OrderDetailRepository(ILogger<OrderDetailRepository> logger,
            IOrderRepository ordeRepo,
            IOrderDatabaseCalls orderDbCalls,
            IExpressionFunction expFunc)
        {
            _orderRepo = ordeRepo;
            _orderDbCalls = orderDbCalls;
            _expFunc = expFunc;
            _logger = logger;
        }

        public async Task<int> FindOrderDetailsRemovedCountAsync(long id)
        {
            var filterValue = "Order.Order_Id IsRemoved";
            var searchString = "" + id + "&&true";
            var inBetween = "And";
            var operand = "== ==";
            var negate = "false false";

            var query = FilterOrderDetail(filterValue, searchString, inBetween, operand, negate);

            var num = await query.CountAsync();

            return num;
        }

        public async Task<Order> CancelOrder(long id)
        {
            try
            {
                var orderDetails = await FindOrderDetailByOrderIdAsync(id);
                var order = _orderRepo.FindOrderById(id);

                using (var transaction = _orderDbCalls.BeginTransaction())
                {
                    foreach (var detail in orderDetails)
                        if (detail.IsRemoved != true)
                        {
                            order.Subtotal -= detail.Quantity * detail.OrderDetailPrice;
                            order.Tax -= detail.Quantity * detail.OrderDetailPrice * (decimal).1;

                            await _orderDbCalls.ContextSaveChangesAsync();

                            //detail.Price.Quantity += detail.Quantity;
                            detail.IsRemoved = true;

                            await _orderDbCalls.ContextSaveChangesAsync();
                        }

                    order.OrderStatus = OrderStatus.Cancelled;

                    await _orderDbCalls.ContextSaveChangesAsync();

                    await _orderDbCalls.TransactionCommitChangesAsync(transaction);

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

        public async Task<Dictionary<string, string>> MakeOrderDetailInactive(long id, long orderId, int quantity)
        {
            try
            {
                var origOrderDetail = await FindOrderDetailByIdAsync(id);

                var moneyOwe = origOrderDetail.OrderDetailPrice * origOrderDetail.Quantity;

                var origOrder = _orderRepo.FindOrderById(orderId);

                //if (origOrderDetail.IsRemoved || origOrder.OrderStatus.Position > pending.Position) return null;

                using (var transaction = _orderDbCalls.BeginTransaction())
                {
                    origOrder.Subtotal -= moneyOwe;

                    origOrder.Tax -= moneyOwe * (decimal).10;

                    await _orderDbCalls.ContextSaveChangesAsync();

                    origOrderDetail.IsRemoved = true;

                    //origOrderDetail.Price.Quantity += quantity;


                    await _orderDbCalls.ContextSaveChangesAsync();

                    await _orderDbCalls.TransactionCommitChangesAsync(transaction);
                    var emailInfo = new Dictionary<string, string>
                    {
                        { "bookName", origOrderDetail.Book.Name },
                        //{ "bookCondition", origOrderDetail.Price.Condition.ConditionName },
                        { "customerFirstName", origOrder.Customer.FirstName },
                        { "customerEmail", origOrder.Customer.Email }
                    };
                    return emailInfo;
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

        // Finds one instance of Order Detail Model by Order Detail Id
        public async Task<OrderDetail> FindOrderDetailByIdAsync(long id)
        {
            var filterValue = "OrderDetail_Id";
            var searchString = "" + id;
            var inBetween = "";
            var operand = "==";
            var negate = "false";

            var query = FilterOrderDetail(filterValue, searchString, inBetween, operand, negate);

            var orderDetail = await query.FirstAsync();

            return orderDetail;
        }

        // Finds a List of Order Details by the associated Order Id
        public async Task<List<OrderDetail>> FindOrderDetailByOrderIdAsync(long orderId)
        {
            var filterValue = "Order.Id";
            var searchString = "" + orderId;
            var inBetween = "";
            var operand = "==";
            var negate = "false";

            var query = FilterOrderDetail(filterValue, searchString, inBetween, operand, negate);

            var orderDetail = await query.ToListAsync();

            return orderDetail;
        }

        public IQueryable<OrderDetail> FilterOrderDetail(string filterValue, string searchString, string inBetween,
            string operand, string negate)
        {
            var tableName = "OrderDetail";

            var lambda = _expFunc.ReturnLambdaExpression<OrderDetail>(tableName, filterValue, searchString, inBetween,
                operand, negate);

            var orderDetailBase = _orderDbCalls.GetBaseQuery("Bookstore.Domain.Orders.OrderDetail");

            var query = _orderDbCalls.ReturnBaseQuery<OrderDetail>(orderDetailBase, OrderDetailIncludes);

            var filterQuery = _orderDbCalls.ReturnFilterQuery(query, lambda);

            return filterQuery;
        }
    }
}
