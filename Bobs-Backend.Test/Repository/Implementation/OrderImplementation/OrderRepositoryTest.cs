using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository.Implementations.OrderImplementations;
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
        private Mock<ISearchRepository> _searchMockRepo;

        [Fact]
        public async Task FindOrderById()
        {
            _searchMockRepo = new Mock<ISearchRepository>();

            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            _orderRepo = new OrderRepository(context, _searchMockRepo.Object);

            var testOrder = new Order();
            testOrder.Order_Id = 23;
            
            context.Order.Add(testOrder);

            await context.SaveChangesAsync();

            var orderResult = await _orderRepo.FindOrderById((long)23);

            Assert.NotNull(orderResult);

            Assert.Equal("23", orderResult.Order_Id + "");

            await context.DisposeAsync();
        }


        [Fact]
        public async Task GetAllOrders()
        {
            _searchMockRepo = new Mock<ISearchRepository>();

            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            _orderRepo = new OrderRepository(context, _searchMockRepo.Object);

            var testOrder = new Order();
            testOrder.Order_Id = 23;

            var testOrder1 = new Order();
            testOrder1.Order_Id = 24;

            context.Order.Add(testOrder);

            context.Order.Add(testOrder1);

            var orderQuery = (IQueryable<Order>)context.Order;

            var totalPages = 1;
            var pages = new int[1] { 1 };

            _searchMockRepo.Setup(r => r.GetBaseQuery("BOBS_Backend.Models.Order.Order")).Returns(orderQuery);
            _searchMockRepo.Setup(r => r.GetTotalPages(2, 15)).Returns(totalPages);
            _searchMockRepo.Setup(r => r.GetModifiedPagesArr(1, 1)).Returns(pages);

            await context.SaveChangesAsync();

            var orderResult = await _orderRepo.GetAllOrders(1);

            Assert.NotNull(orderResult);

            Assert.Equal("2",orderResult.Orders.Count()+"");
            Assert.Equal("False", orderResult.HasNextPages + "");

            await context.DisposeAsync();
        }

        //Still Work in Progress
        //[Fact]
        //public async Task FilterList_WhenLambdaNotNull()
        //{
        //    _searchMockRepo = new Mock<ISearchRepository>();

        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _orderRepo = new OrderRepository(context, _searchMockRepo.Object);

        //    var testOrder = new Order();
        //    testOrder.Order_Id = 23;

        //    var testOrder1 = new Order();
        //    testOrder1.Order_Id = 24;

        //    context.Order.Add(testOrder);

        //    context.Order.Add(testOrder1);

        //    await context.SaveChangesAsync();

        //    var orderQuery = (IQueryable<Order>)context.Order;

        //    var totalPages = 1;
        //    var pages = new int[1] { 1 };
        //    var filterValue = " Order_Id";
        //    var searchString = "24";

        //    var parameterExpression = Expression.Parameter(typeof(BOBS_Backend.Models.Order.Order), "order");
      

        //    var property = Expression.Property(parameterExpression, "Order_Id");
        //    var constant = Expression.Constant((long)24);

        //    var expResult = Expression.Equal(property, constant);
  

        //    _searchMockRepo.Setup(r => r.GetBaseQuery("BOBS_Backend.Models.Order.Order")).Returns(orderQuery);
        //    _searchMockRepo.Setup(r => r.GetTotalPages(1, 15)).Returns(totalPages);
        //    _searchMockRepo.Setup(r => r.GetModifiedPagesArr(1, 1)).Returns(pages);
        //    _searchMockRepo.Setup(r => r.ReturnExpression(parameterExpression, filterValue, searchString)).Returns(expResult);

        //    await context.SaveChangesAsync();

        //    var filteredOrder = await _orderRepo.FilterList(filterValue, searchString, 1);

        //    Assert.NotNull(filteredOrder);

        //    await context.DisposeAsync();

        //}
    }
}
