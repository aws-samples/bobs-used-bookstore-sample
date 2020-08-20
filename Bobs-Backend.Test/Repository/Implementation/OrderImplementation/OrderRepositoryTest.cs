using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository.Implementations.OrderImplementations;
using BOBS_Backend.Repository.OrdersInterface;
using BOBS_Backend.Repository.SearchImplementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bobs_Backend.Test.Repository.Implementation.OrderImplementation
{
    public class OrderRepositoryTest
    {

        private OrderRepository _orderRepo;
        private readonly string[] OrderIncludes = { "Customer", "Address", "OrderStatus" };

        [Fact]
        public async Task FindOrderById()
        {
            var orderDbCallsMock = new Mock<IOrderDatabaseCalls>();
            var expFuncMock = new Mock<IExpressionFunction>();
            var searchRepoMock = new Mock<ISearchRepository>();

            _orderRepo = new OrderRepository(searchRepoMock.Object, orderDbCallsMock.Object, expFuncMock.Object);

            var testOrder = new Order();
            testOrder.Order_Id = 23;

            var testOrder1 = new Order();
            testOrder1.Order_Id = 24;

            List<Order> data = new List<Order>();
            data.Add(testOrder);
            data.Add(testOrder1);

            List<Order> filterData = new List<Order>();
            filterData.Add(testOrder);

            var parameterExpression = Expression.Parameter(typeof(Order), "Order");
            string filterValue = "Order_Id";
            string searchString = "" + 23;
            string inBetween = "";
            string operand = "==";
            string negate = "false";
            string tableName = "Order";
            IQueryable<Order> orderQuery = data.AsQueryable();
            var property = Expression.Property(parameterExpression, "Order_Id");
            var constant = Expression.Constant((long)23);
            var expression = Expression.Equal(property, constant);

            Expression<Func<Order, bool>> lambda = Expression.Lambda<Func<Order, bool>>(expression, parameterExpression);

            expFuncMock.Setup(exp => exp.ReturnLambdaExpression<Order>(tableName, filterValue, searchString, inBetween, operand, negate)).Returns(lambda);

            orderDbCallsMock.Setup(db => db.GetBaseQuery("BOBS_Backend.Models.Order.Order")).Returns(orderQuery);
            orderDbCallsMock.Setup(db => db.ReturnBaseQuery<Order>(orderQuery, OrderIncludes)).Returns(orderQuery);
            orderDbCallsMock.Setup(db => db.ReturnFilterQuery(orderQuery, lambda)).Returns(filterData.AsQueryable());

            var orderResult = await _orderRepo.FindOrderById((long)23);

            Assert.NotNull(orderResult);

            Assert.Equal("23", orderResult.Order_Id + "");


        }


        [Fact]
        public async Task GetAllOrders()
        {
            var orderDbCallsMock = new Mock<IOrderDatabaseCalls>();
            var expFuncMock = new Mock<IExpressionFunction>();
            var searchRepoMock = new Mock<ISearchRepository>();

            _orderRepo = new OrderRepository(searchRepoMock.Object, orderDbCallsMock.Object, expFuncMock.Object);

            var testOrder = new Order();
            testOrder.Order_Id = 23;
            testOrder.OrderStatus = new OrderStatus
            {
                OrderStatus_Id = 1,
                Status = "Just Placed",
                position = 1
            };

            var testOrder1 = new Order();
            testOrder1.Order_Id = 24;
            testOrder1.OrderStatus = new OrderStatus
            {
                OrderStatus_Id = 2,
                Status = "Pending",
                position = 2
            };

            List<Order> data = new List<Order>();
            data.Add(testOrder1);
            data.Add(testOrder);


            var totalPages = 1;
            var pages = new int[1] { 1 };

            var orderQuery = data.AsQueryable();

            orderDbCallsMock.Setup(db => db.GetBaseQuery("BOBS_Backend.Models.Order.Order")).Returns(orderQuery);
            orderDbCallsMock.Setup(db => db.ReturnBaseQuery<Order>(orderQuery, OrderIncludes)).Returns(orderQuery);


            searchRepoMock.Setup(r => r.GetTotalPages(2, 20)).Returns(totalPages);
            searchRepoMock.Setup(r => r.GetModifiedPagesArr(1, 1)).Returns(pages);


            var orderResult = await _orderRepo.GetAllOrders(1);

            Assert.NotNull(orderResult);

            Assert.Equal("2", orderResult.Orders.Count() + "");
            Assert.Equal("1", orderResult.Orders[0].OrderStatus.position + "");
            Assert.Equal("False", orderResult.HasNextPages + "");


        }


        [Fact]
        public async Task FilterList_WhenLambdaNotNull()
        {
            var orderDbCallsMock = new Mock<IOrderDatabaseCalls>();
            var expFuncMock = new Mock<IExpressionFunction>();
            var searchRepoMock = new Mock<ISearchRepository>();

            _orderRepo = new OrderRepository(searchRepoMock.Object, orderDbCallsMock.Object, expFuncMock.Object);

            var testOrder = new Order();
            testOrder.Order_Id = 23;
            testOrder.OrderStatus = new OrderStatus
            {
                OrderStatus_Id = 1,
                Status = "Just Placed",
                position = 1
            };

            var testOrder1 = new Order();
            testOrder1.Order_Id = 24;
            testOrder1.OrderStatus = new OrderStatus
            {
                OrderStatus_Id = 2,
                Status = "Pending",
                position = 2
            };

            List<Order> data = new List<Order>();
            data.Add(testOrder1);
            data.Add(testOrder);

            List<Order> filterData = new List<Order>();
            filterData.Add(testOrder1);

            var orderQuery = data.AsQueryable();
            var filterOrderQuery = filterData.AsQueryable();

            var totalPages = 1;
            var pages = new int[1] { 1 };
            var filterValue = " Order_Id";
            var searchString = "24";

            var parameterExpression = Expression.Parameter(typeof(Order), "Order");


            var property = Expression.Property(parameterExpression, "Order_Id");
            var constant = Expression.Constant((long)24);

            var expResult = Expression.Equal(property, constant);

            var lambda = Expression.Lambda < Func<Order, bool>>(expResult, parameterExpression);

            orderDbCallsMock.Setup(r => r.GetBaseQuery("BOBS_Backend.Models.Order.Order")).Returns(orderQuery);
            orderDbCallsMock.Setup(db => db.ReturnBaseQuery<Order>(orderQuery, OrderIncludes)).Returns(orderQuery);
            orderDbCallsMock.Setup(db => db.ReturnFilterQuery<Order>(orderQuery, lambda)).Returns(filterOrderQuery);

            expFuncMock.Setup(exp => exp.ReturnParameterExpression(typeof(Order), "Order")).Returns(parameterExpression);
            expFuncMock.Setup(exp => exp.ReturnLambdaExpression<Order>(expResult, parameterExpression)).Returns(lambda);

            searchRepoMock.Setup(r => r.GetTotalPages(1, 20)).Returns(totalPages);
            searchRepoMock.Setup(r => r.GetModifiedPagesArr(1, 1)).Returns(pages);
            searchRepoMock.Setup(r => r.ReturnExpression(parameterExpression, filterValue, searchString)).Returns(expResult);


            var filteredOrder = await _orderRepo.FilterList(filterValue, searchString, 1);

            Assert.NotNull(filteredOrder);

            Assert.Equal("1", filteredOrder.Orders.Count() + "");
            Assert.Equal(filterValue, filteredOrder.FilterValue);
            Assert.Equal(searchString, filteredOrder.SearchString);
        }

        [Fact]
        public async Task FilterList_WhenLambdaIsNull()
        {
            var orderDbCallsMock = new Mock<IOrderDatabaseCalls>();
            var expFuncMock = new Mock<IExpressionFunction>();
            var searchRepoMock = new Mock<ISearchRepository>();

            _orderRepo = new OrderRepository(searchRepoMock.Object, orderDbCallsMock.Object, expFuncMock.Object);

            var testOrder = new Order();
            testOrder.Order_Id = 23;
            testOrder.OrderStatus = new OrderStatus
            {
                OrderStatus_Id = 1,
                Status = "Just Placed",
                position = 1
            };

            var testOrder1 = new Order();
            testOrder1.Order_Id = 24;
            testOrder1.OrderStatus = new OrderStatus
            {
                OrderStatus_Id = 2,
                Status = "Pending",
                position = 2
            };

            List<Order> data = new List<Order>();
            data.Add(testOrder1);
            data.Add(testOrder);

            List<Order> filterData = new List<Order>();
            filterData.Add(testOrder1);

            var orderQuery = data.AsQueryable();
            var filterOrderQuery = filterData.AsQueryable();

            var filterValue = " Order_Id";
            var searchString = "24";

            var parameterExpression = Expression.Parameter(typeof(Order), "Order");


            var property = Expression.Property(parameterExpression, "Order_Id");
            var constant = Expression.Constant((long)24);

            var expResult = Expression.Equal(property, constant);

            Expression<Func<Order,bool>> lambda = null;

            expFuncMock.Setup(exp => exp.ReturnParameterExpression(typeof(Order), "Order")).Returns(parameterExpression);
            expFuncMock.Setup(exp => exp.ReturnLambdaExpression<Order>(expResult, parameterExpression)).Returns(lambda);

            searchRepoMock.Setup(r => r.ReturnExpression(parameterExpression, filterValue, searchString)).Returns(expResult);


            var filteredOrder = await _orderRepo.FilterList(filterValue, searchString, 1);

            Assert.NotNull(filteredOrder);

            Assert.Null(filteredOrder.Orders);
            Assert.Equal("", filteredOrder.FilterValue);
            Assert.Equal("", filteredOrder.SearchString);
        }
    }
}
