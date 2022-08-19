using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Interface.OrdersInterface;
using DataAccess.Repository.Interface.SearchImplementations;
using DataModels.Orders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository.Implementation.OrderImplementations
{
    // Order Status Repository contains all functions associated with OrderStatus Model
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private const string OrderStatusJustPlaced = "Just Placed";
        private readonly IExpressionFunction _expFunc;
        private readonly IOrderDatabaseCalls _orderDbCalls;

        public OrderStatusRepository(IOrderDatabaseCalls orderDbCalls, IExpressionFunction expFunc)
        {
            _orderDbCalls = orderDbCalls;
            _expFunc = expFunc;
        }

        // Return a single order status instance by Order Status Id
        public OrderStatus FindOrderStatusById(long id)
        {
            var filterValue = "OrderStatus_Id";
            var searchString = "" + id;
            var inBetween = "";
            var operand = "==";
            var negate = "false";

            var query = FilterOrderStatus(filterValue, searchString, inBetween, operand, negate);

            var orderStatus = query.First();

            return orderStatus;
        }

        public OrderStatus FindOrderStatusByName(string status)
        {
            var filterValue = "Status";
            var searchString = status;
            var inBetween = "";
            var operand = "==";
            var negate = "false";

            var query = FilterOrderStatus(filterValue, searchString, inBetween, operand, negate);

            var orderStatus = query.FirstOrDefault();
            return orderStatus;
        }

        // Returns all Order Statuses in a Table
        public List<OrderStatus> GetOrderStatuses()
        {
            var orderBase = _orderDbCalls.GetBaseQuery("DataModels.Orders.OrderStatus");

            var query = (IQueryable<OrderStatus>)orderBase;

            var orderStatus = query
                .OrderBy(os => os.Position)
                .ToList();

            return orderStatus;
        }

        // Updates the status of an Order Instance 
        public async Task<Order> UpdateOrderStatus(Order order, long Status_Id)
        {
            try
            {
                var newStatus = FindOrderStatusById(Status_Id);

                order.OrderStatus = newStatus;

                if (order.OrderStatus.Status != OrderStatusJustPlaced && order.DeliveryDate == null)
                    order.DeliveryDate = DateTime.Now.ToUniversalTime().AddDays(14).ToString();

                await _orderDbCalls.ContextSaveChanges();

                return order;
            }
            catch (DbUpdateConcurrencyException)
            {
                // todo: log exception
                return null;
            }
            catch (Exception)
            {
                // todo: log exception
                return null;
            }
        }


        public IQueryable<OrderStatus> FilterOrderStatus(string filterValue, string searchString, string inBetween,
            string operand, string negate)
        {
            var tableName = "OrderStatus";

            var lambda = _expFunc.ReturnLambdaExpression<OrderStatus>(tableName, filterValue, searchString, inBetween,
                operand, negate);

            var orderStatusBase = _orderDbCalls.GetBaseQuery("DataModels.Orders.OrderStatus");

            var query = (IQueryable<OrderStatus>)orderStatusBase;

            var filterQuery = _orderDbCalls.ReturnFilterQuery(query, lambda);

            return filterQuery;
        }
    }
}