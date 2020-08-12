using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository.Implementations.OrderImplementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bobs_Backend.Test.Repository.Implementation.OrderImplementation
{
    public class OrderStatusRepositoryTest
    {

        private OrderStatusRepository _orderStatusRepo;

        [Fact]
        public async Task FindOrderStatusById_WhenOrderStatusExist()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            long id = 1;
            var testData = new OrderStatus();
            testData.OrderStatus_Id = id;


            context.OrderStatus.Add(testData);
            await context.SaveChangesAsync();

            _orderStatusRepo = new OrderStatusRepository(context);

            var orderStatus = await _orderStatusRepo.FindOrderStatusById(id);

            Assert.NotNull(orderStatus);
            Assert.Equal(id + "", orderStatus.OrderStatus_Id + "");

            await context.DisposeAsync();
        }


        [Fact]
        public async Task FindOrderStatusByName_WhenOrderStatusExist()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            var testData = new OrderStatus();
            testData.OrderStatus_Id = 1;
            testData.Status = "Just Placed";

            context.OrderStatus.Add(testData);
            await context.SaveChangesAsync();

            _orderStatusRepo = new OrderStatusRepository(context);

            var orderStatus = await _orderStatusRepo.FindOrderStatusByName("Just Placed");

            Assert.NotNull(orderStatus);
            Assert.Equal("Just Placed",orderStatus.Status);

            await context.DisposeAsync();
        }

        //[Fact]
        //public async Task GetOrderStatuses()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    var testData = new OrderStatus();
        //    testData.OrderStatus_Id = 1;
        //    testData.Status = "Just Placed";

        //    var testData2 = new OrderStatus();
        //    testData.OrderStatus_Id = 2;
        //    testData.Status = "Pending";

        //    var testData3 = new OrderStatus();
        //    testData.OrderStatus_Id = 3;
        //    testData.Status = "Delivered";

        //    context.OrderStatus.Add(testData);
        //    context.OrderStatus.Add(testData2);
        //    context.OrderStatus.Add(testData3);

        //    await context.SaveChangesAsync();

        //    _orderStatusRepo = new OrderStatusRepository(context);

        //    var allOrderStatus = await _orderStatusRepo.
        //}
    }
}
