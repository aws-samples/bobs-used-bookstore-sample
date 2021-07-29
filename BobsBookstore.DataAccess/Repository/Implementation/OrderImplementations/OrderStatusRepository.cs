using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BobsBookstore.Models.Orders;
using BobsBookstore.DataAccess.Repository.Interface.OrdersInterface;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;

namespace BobsBookstore.DataAccess.Repository.Implementation.OrderImplementations
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        /*
         * Order Status Repository contains all functions associated with OrderStatus Model
         */

        private IOrderDatabaseCalls _orderDbCalls;
        private IExpressionFunction _expFunc;


        // Set up Database Connection
        public OrderStatusRepository(IOrderDatabaseCalls orderDbCalls, IExpressionFunction expFunc)
        {

            _orderDbCalls = orderDbCalls;
            _expFunc = expFunc;
        }

        // Return a Singel Order Status Instance by Order Status Id
        public OrderStatus FindOrderStatusById(long id)
        {

            string filterValue = "OrderStatus_Id";
            string searchString = "" + id;
            string inBetween = "";
            string operand = "==";
            string negate = "false";

            var query = FilterOrderStatus(filterValue, searchString, inBetween, operand, negate);

            var orderStatus = query.First();

            return orderStatus;
        }

        public OrderStatus FindOrderStatusByName(string status)
        {
            string filterValue = "Status";
            string searchString = status;
            string inBetween = "";
            string operand = "==";
            string negate = "false";

            var query = FilterOrderStatus(filterValue, searchString, inBetween, operand, negate);


            var orderStatus = query.FirstOrDefault();
            return orderStatus;



        }

        // Returns all Order Statuses in a Table
        public List<OrderStatus> GetOrderStatuses()
        {
            var orderBase = _orderDbCalls.GetBaseQuery("BobsBookstore.Models.Orders.OrderStatus");

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
                OrderStatus newStatus = FindOrderStatusById(Status_Id);

                order.OrderStatus = newStatus;

                if (order.OrderStatus.Status != "Just Placed" && order.DeliveryDate == null) order.DeliveryDate = DateTime.Now.ToUniversalTime().AddDays(14).ToString();

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


        public IQueryable<OrderStatus> FilterOrderStatus(string filterValue, string searchString, string inBetween, string operand, string negate)
        {

            string tableName = "OrderStatus";

            Expression<Func<OrderStatus, bool>> lambda = _expFunc.ReturnLambdaExpression<OrderStatus>(tableName, filterValue, searchString, inBetween, operand, negate);

            var orderStatusBase = _orderDbCalls.GetBaseQuery("BobsBookstore.Models.Orders.OrderStatus");

            var query = (IQueryable<OrderStatus>) orderStatusBase;

            var filterQuery = _orderDbCalls.ReturnFilterQuery(query,lambda);

            return filterQuery;
        }
    }
}
