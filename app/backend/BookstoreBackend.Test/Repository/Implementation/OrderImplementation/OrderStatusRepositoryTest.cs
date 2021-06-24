using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository;
using BOBS_Backend.Repository.Implementations.InventoryImplementation;
using BOBS_Backend.Repository.Implementations.OrderImplementations;
using BOBS_Backend.Repository.OrdersInterface;
using BOBS_Backend.Repository.SearchImplementations;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;
using Microsoft.EntityFrameworkCore;
namespace Bobs_Backend.Test.Repository.Implementation.OrderImplementation
{
    public class OrderStatusRepositoryTest
    {

        private OrderStatusRepository _orderStatusRepo;

        [Fact]
        public async Task FindOrderStatusById_WhenOrderStatusExist()
        {


            var orderDbCallsMock = new Mock<IOrderDatabaseCalls>();
            var expFuncMock = new Mock<IExpressionFunction>();


            _orderStatusRepo = new OrderStatusRepository(orderDbCallsMock.Object, expFuncMock.Object);

            long id = 1;
            var testData = new OrderStatus();
            testData.OrderStatus_Id = id;
            var testData1 = new OrderStatus();
            testData1.OrderStatus_Id = 3;

            List<OrderStatus> data = new List<OrderStatus>();
            data.Add(testData);
            data.Add(testData1);

            List<OrderStatus> filterData = new List<OrderStatus>();
           filterData.Add(testData);

            var parameterExpression = Expression.Parameter(typeof(OrderStatus), "OrderStatus");
            string filterValue = "OrderStatus_Id";
            string searchString = "" + id;
            string inBetween = "";
            string operand = "==";
            string negate = "false";
            string tableName = "OrderStatus";
            IQueryable<OrderStatus> orderStatusQuery = data.AsQueryable();
            var property = Expression.Property(parameterExpression, "OrderStatus_Id");
            var constant = Expression.Constant(id);
            var expression = Expression.Equal(property, constant);

            Expression<Func<OrderStatus, bool>> lambda = Expression.Lambda<Func<OrderStatus, bool>>(expression, parameterExpression);

          
            expFuncMock.Setup(exp => exp.ReturnLambdaExpression<OrderStatus>(tableName, filterValue, searchString, inBetween, operand, negate)).Returns(lambda);

            orderDbCallsMock.Setup(db => db.GetBaseQuery("BOBS_Backend.Models.Order.OrderStatus")).Returns(orderStatusQuery);
            orderDbCallsMock.Setup(db => db.ReturnFilterQuery(orderStatusQuery, lambda)).Returns(filterData.AsQueryable());

            var orderStatus = await _orderStatusRepo.FindOrderStatusById(id);

            Assert.NotNull(orderStatus);
            Assert.Equal(id + "", orderStatus.OrderStatus_Id + "");


        }


        [Fact]
        public async Task FindOrderStatusByName_WhenOrderStatusExist()
        {

            var orderDbCallsMock = new Mock<IOrderDatabaseCalls>();
            var expFuncMock = new Mock<IExpressionFunction>();


            _orderStatusRepo = new OrderStatusRepository(orderDbCallsMock.Object, expFuncMock.Object);

            long id = 1;
            var testData = new OrderStatus();
            testData.OrderStatus_Id = id;
            testData.Status = "Just Placed";
            var testData1 = new OrderStatus();
            testData1.OrderStatus_Id = 3;
            testData1.Status = "Pending";

            List<OrderStatus> data = new List<OrderStatus>();
            data.Add(testData);
            data.Add(testData1);

            List<OrderStatus> filterData = new List<OrderStatus>();
            filterData.Add(testData1);

            var test = Expression.GetFuncType(typeof(OrderStatus), typeof(bool));
            var parameterExpression = Expression.Parameter(typeof(OrderStatus), "OrderStatus");
            string filterValue = "Status";
            string searchString = "Pending";
            string inBetween = "";
            string operand = "==";
            string negate = "false";
            string tableName = "OrderStatus";
            IQueryable<OrderStatus> orderStatusQuery = data.AsQueryable();
            var property = Expression.Property(parameterExpression, "Status");
            var constant = Expression.Constant("Pending");
            var expression = Expression.Equal(property, constant);

            Expression<Func<OrderStatus, bool>> lambda = Expression.Lambda<Func<OrderStatus, bool>>(expression, parameterExpression);


            expFuncMock.Setup(exp => exp.ReturnLambdaExpression<OrderStatus>(tableName, filterValue, searchString, inBetween, operand, negate)).Returns(lambda);

            orderDbCallsMock.Setup(db => db.GetBaseQuery("BOBS_Backend.Models.Order.OrderStatus")).Returns(orderStatusQuery);
            orderDbCallsMock.Setup(db => db.ReturnFilterQuery(orderStatusQuery, lambda)).Returns(filterData.AsQueryable());

            var orderStatus = await _orderStatusRepo.FindOrderStatusByName("Pending");

            Assert.NotNull(orderStatus);
            Assert.Equal("Pending", orderStatus.Status);

        }

        [Fact]
        public async Task GetOrderStatuses()
        {
            var orderDbCallsMock = new Mock<IOrderDatabaseCalls>();
            var expFuncMock = new Mock<IExpressionFunction>();


            _orderStatusRepo = new OrderStatusRepository(orderDbCallsMock.Object, expFuncMock.Object);

            var testData = new OrderStatus();
            testData.OrderStatus_Id = 1;
            testData.Status = "Just Placed";
            testData.position = 1;

            var testData2 = new OrderStatus();
            testData2.OrderStatus_Id = 2;
            testData2.Status = "Pending";
            testData2.position = 2;

            var testData3 = new OrderStatus();
            testData3.OrderStatus_Id = 3;
            testData3.Status = "Delivered";
            testData3.position = 3;

            List<OrderStatus> data = new List<OrderStatus>();
            data.Add(testData2);
            data.Add(testData);
            data.Add(testData3);

            orderDbCallsMock.Setup(db => db.GetBaseQuery("BOBS_Backend.Models.Order.OrderStatus")).Returns(data.AsQueryable());

            var allOrderStatus = await _orderStatusRepo.GetOrderStatuses();

            Assert.NotNull(allOrderStatus);

            Assert.Equal("3", "" + allOrderStatus.Count());
            Assert.Equal("1", "" + allOrderStatus[0].position);


    
        }

        [Fact]
        public async Task UpdateOrderStatus_WhenDeliveryDateIsNull()
        {


            var orderStatusTest1 = new OrderStatus();
            orderStatusTest1.OrderStatus_Id = 1;
            orderStatusTest1.Status = "Just Placed";

            var orderStatusTest2 = new OrderStatus();
            orderStatusTest2.OrderStatus_Id = 2;
            orderStatusTest2.Status = "Pending";

            var order = new Order();
            order.OrderStatus = orderStatusTest1;
            order.DeliveryDate = null;

            List<OrderStatus> data = new List<OrderStatus>();
            data.Add(orderStatusTest1);
            data.Add(orderStatusTest2);

            List<OrderStatus> filterData = new List<OrderStatus>();
            filterData.Add(orderStatusTest2);

            var orderDbCallsMock = new Mock<IOrderDatabaseCalls>();
            var expFuncMock = new Mock<IExpressionFunction>();


            _orderStatusRepo = new OrderStatusRepository(orderDbCallsMock.Object, expFuncMock.Object);

            long id = 2;
            var parameterExpression = Expression.Parameter(typeof(OrderStatus), "OrderStatus");
            string filterValue = "OrderStatus_Id";
            string searchString = "" + id;
            string inBetween = "";
            string operand = "==";
            string negate = "false";
            string tableName = "OrderStatus";
            IQueryable<OrderStatus> orderStatusQuery = data.AsQueryable();
            var property = Expression.Property(parameterExpression, "OrderStatus_Id");
            var constant = Expression.Constant(id);
            var expression = Expression.Equal(property, constant);

            Expression<Func<OrderStatus, bool>> lambda = Expression.Lambda<Func<OrderStatus, bool>>(expression, parameterExpression);


            expFuncMock.Setup(exp => exp.ReturnLambdaExpression<OrderStatus>(tableName, filterValue, searchString, inBetween, operand, negate)).Returns(lambda);

            orderDbCallsMock.Setup(db => db.GetBaseQuery("BOBS_Backend.Models.Order.OrderStatus")).Returns(orderStatusQuery);
            orderDbCallsMock.Setup(db => db.ReturnFilterQuery(orderStatusQuery, lambda)).Returns(filterData.AsQueryable());


            var updatedOrder = await _orderStatusRepo.UpdateOrderStatus(order, orderStatusTest2.OrderStatus_Id);

            Assert.NotNull(updatedOrder);
            Assert.NotNull(updatedOrder.DeliveryDate);
            Assert.Equal(orderStatusTest2.Status, updatedOrder.OrderStatus.Status);


        }

        [Fact]
        public async Task UpdateOrderStatus_WhenDeliveryDateIsNotNull()
        {
            var orderStatusTest1 = new OrderStatus();
            orderStatusTest1.OrderStatus_Id = 1;
            orderStatusTest1.Status = "Just Placed";

            var orderStatusTest2 = new OrderStatus();
            orderStatusTest2.OrderStatus_Id = 2;
            orderStatusTest2.Status = "Pending";

            var order = new Order();
            order.OrderStatus = orderStatusTest1;
            order.DeliveryDate = "fdefefef";

            List<OrderStatus> data = new List<OrderStatus>();
            data.Add(orderStatusTest1);
            data.Add(orderStatusTest2);

            List<OrderStatus> filterData = new List<OrderStatus>();
            filterData.Add(orderStatusTest2);

            var orderDbCallsMock = new Mock<IOrderDatabaseCalls>();
            var expFuncMock = new Mock<IExpressionFunction>();


            _orderStatusRepo = new OrderStatusRepository(orderDbCallsMock.Object, expFuncMock.Object);

            long id = 2;
            var parameterExpression = Expression.Parameter(typeof(OrderStatus), "OrderStatus");
            string filterValue = "OrderStatus_Id";
            string searchString = "" + id;
            string inBetween = "";
            string operand = "==";
            string negate = "false";
            string tableName = "OrderStatus";
            IQueryable<OrderStatus> orderStatusQuery = data.AsQueryable();
            var property = Expression.Property(parameterExpression, "OrderStatus_Id");
            var constant = Expression.Constant(id);
            var expression = Expression.Equal(property, constant);

            Expression<Func<OrderStatus, bool>> lambda = Expression.Lambda<Func<OrderStatus, bool>>(expression, parameterExpression);


            expFuncMock.Setup(exp => exp.ReturnLambdaExpression<OrderStatus>(tableName, filterValue, searchString, inBetween, operand, negate)).Returns(lambda);

            orderDbCallsMock.Setup(db => db.GetBaseQuery("BOBS_Backend.Models.Order.OrderStatus")).Returns(orderStatusQuery);
            orderDbCallsMock.Setup(db => db.ReturnFilterQuery(orderStatusQuery, lambda)).Returns(filterData.AsQueryable());

            var updatedOrder = await _orderStatusRepo.UpdateOrderStatus(order, orderStatusTest2.OrderStatus_Id);

            Assert.NotNull(updatedOrder);
            Assert.Equal(orderStatusTest2.Status, updatedOrder.OrderStatus.Status);


        }
    }
}
