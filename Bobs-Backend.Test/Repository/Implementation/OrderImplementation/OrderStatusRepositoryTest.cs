using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository;
using BOBS_Backend.Repository.Implementations.InventoryImplementation;
using BOBS_Backend.Repository.Implementations.OrderImplementations;
using BOBS_Backend.Repository.OrdersInterface;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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

        [Fact]
        public async Task GetOrderStatuses()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            var testData = new OrderStatus();
            testData.OrderStatus_Id = 1;
            testData.Status = "Just Placed";

            var testData2 = new OrderStatus();
            testData.OrderStatus_Id = 2;
            testData.Status = "Pending";

            var testData3 = new OrderStatus();
            testData.OrderStatus_Id = 3;
            testData.Status = "Delivered";

            context.OrderStatus.Add(testData);
            context.OrderStatus.Add(testData2);
            context.OrderStatus.Add(testData3);

            await context.SaveChangesAsync();

            _orderStatusRepo = new OrderStatusRepository(context);

            var allOrderStatus = await _orderStatusRepo.GetOrderStatuses();

            Assert.NotNull(allOrderStatus);

            Assert.Equal("3", "" + allOrderStatus.Count());

            await context.DisposeAsync();
        }

        [Fact]
        public async Task UpdateOrderStatus_WhenDeliveryDateIsNull()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            var orderStatusTest1 = new OrderStatus();
            orderStatusTest1.OrderStatus_Id = 1;
            orderStatusTest1.Status = "Just Placed";

            var orderStatusTest2 = new OrderStatus();
            orderStatusTest2.OrderStatus_Id = 2;
            orderStatusTest2.Status = "Pending";

            var order = new Order();
            order.OrderStatus = orderStatusTest1;
            order.DeliveryDate = null;

            context.Order.Add(order);
            context.OrderStatus.Add(orderStatusTest1);
            context.OrderStatus.Add(orderStatusTest2);

            await context.SaveChangesAsync();


            _orderStatusRepo = new OrderStatusRepository(context);
           

            var updatedOrder = await _orderStatusRepo.UpdateOrderStatus(order, orderStatusTest2.OrderStatus_Id);

            Assert.NotNull(updatedOrder);
            Assert.NotNull(updatedOrder.DeliveryDate);
            Assert.Equal(orderStatusTest2.Status, updatedOrder.OrderStatus.Status);

            await context.DisposeAsync();
            


        }

        [Fact]
        public async Task UpdateOrderStatus_WhenDeliveryDateIsNotNull()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            var orderStatusTest1 = new OrderStatus();
            orderStatusTest1.OrderStatus_Id = 1;
            orderStatusTest1.Status = "Just Placed";

            var orderStatusTest2 = new OrderStatus();
            orderStatusTest2.OrderStatus_Id = 2;
            orderStatusTest2.Status = "Pending";

            var order = new Order();
            order.OrderStatus = orderStatusTest1;
            order.DeliveryDate = "tetsgd";

            context.Order.Add(order);
            context.OrderStatus.Add(orderStatusTest1);
            context.OrderStatus.Add(orderStatusTest2);

            await context.SaveChangesAsync();


            _orderStatusRepo = new OrderStatusRepository(context);


            var updatedOrder = await _orderStatusRepo.UpdateOrderStatus(order, orderStatusTest2.OrderStatus_Id);

            Assert.NotNull(updatedOrder);
            Assert.Equal(orderStatusTest2.Status, updatedOrder.OrderStatus.Status);

            await context.DisposeAsync();



        }
    }
}
