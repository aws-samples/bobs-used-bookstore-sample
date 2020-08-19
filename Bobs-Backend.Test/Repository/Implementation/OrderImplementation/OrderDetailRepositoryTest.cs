using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository;
using BOBS_Backend.Repository.Implementations.OrderImplementations;
using BOBS_Backend.Repository.OrdersInterface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bobs_Backend.Test.Repository.Implementation.OrderImplementation
{
    public class OrderDetailRepositoryTest
    {

        private OrderDetailRepository _orderDetailRepo;
        private Mock<IOrderRepository> _orderMockRepo;
        private Mock<IOrderStatusRepository> _orderStatusMockRepo;

        //[Fact]
        //public async Task FindOrderDetailsRemovedCountAsync()
        //{
        //    _orderMockRepo = new Mock<IOrderRepository>();
        //    _orderStatusMockRepo = new Mock<IOrderStatusRepository>();

        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    var testOrder = new Order();
        //    testOrder.Order_Id = 9;


        //    var testDetail1 = new OrderDetail();
        //    testDetail1.Order = testOrder;
        //    testDetail1.IsRemoved = true;

        //    var testDetail2 = new OrderDetail();
        //    testDetail2.Order = testOrder;
        //    testDetail2.IsRemoved = false;

        //    context.Add(testOrder);
        //    context.Add(testDetail1);
        //    context.Add(testDetail2);

        //    await context.SaveChangesAsync();

        //    _orderDetailRepo = new OrderDetailRepository(context, _orderMockRepo.Object, _orderStatusMockRepo.Object);

        //    var count = await _orderDetailRepo.FindOrderDetailsRemovedCountAsync(9);

        //    Assert.Equal("1", count+"");

        //}

        //[Fact]
        //public async Task FindOrderDetailsById()
        //{
        //    _orderMockRepo = new Mock<IOrderRepository>();
        //    _orderStatusMockRepo = new Mock<IOrderStatusRepository>();

        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    var testOrder = new Order();
        //    testOrder.Order_Id = 9;


        //    var testDetail1 = new OrderDetail();
        //    testDetail1.Order = testOrder;
        //    testDetail1.OrderDetail_Id = 45;
        //    testDetail1.IsRemoved = true;

        //    var testDetail2 = new OrderDetail();
        //    testDetail2.Order = testOrder;
        //    testDetail2.OrderDetail_Id = 47;
        //    testDetail2.IsRemoved = false;

        //    context.Add(testOrder);
        //    context.Add(testDetail1);
        //    context.Add(testDetail2);

        //    await context.SaveChangesAsync();

        //    _orderDetailRepo = new OrderDetailRepository(context, _orderMockRepo.Object, _orderStatusMockRepo.Object);

        //    var orderDetail = await _orderDetailRepo.FindOrderDetailById((long)47);

        //    Assert.NotNull(orderDetail);
        //    Assert.Equal("47", orderDetail.OrderDetail_Id + "");

        //}

        //[Fact]
        //public async Task FindOrderDetailByOrderId()
        //{
        //    _orderMockRepo = new Mock<IOrderRepository>();
        //    _orderStatusMockRepo = new Mock<IOrderStatusRepository>();

        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    var testOrder = new Order();
        //    testOrder.Order_Id = 9;


        //    var testDetail1 = new OrderDetail();
        //    testDetail1.Order = testOrder;
        //    testDetail1.OrderDetail_Id = 45;
        //    testDetail1.IsRemoved = true;

        //    var testDetail2 = new OrderDetail();
        //    testDetail2.Order = testOrder;
        //    testDetail2.OrderDetail_Id = 47;
        //    testDetail2.IsRemoved = false;

        //    context.Add(testOrder);
        //    context.Add(testDetail1);
        //    context.Add(testDetail2);

        //    await context.SaveChangesAsync();

        //    _orderDetailRepo = new OrderDetailRepository(context, _orderMockRepo.Object, _orderStatusMockRepo.Object);

        //    var orderDetail = await _orderDetailRepo.FindOrderDetailByOrderId((long)9);

        //    Assert.NotNull(orderDetail);
        //    Assert.Equal("2", orderDetail.Count() + "");
        //    Assert.Equal("9", orderDetail[0].Order.Order_Id + "");

        //}
    }
}
