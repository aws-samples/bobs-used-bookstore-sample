using BOBS_Backend.Controllers;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Notifications.NotificationsInterface;
using BOBS_Backend.Repository;
using BOBS_Backend.Repository.OrdersInterface;
using BOBS_Backend.ViewModel.ManageOrders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bobs_Backend.Test.Controllers
{
    public class OrderControllerTest
    {

        public List<Order> ReturnListOfOrders()
        {
            List<Order> orders = new List<Order>();

            orders.Add(new Order()
            {
                Order_Id = 6,
            });
            orders.Add(new Order()
            {
                Order_Id = 7
            });

            return orders;
        }

        public ManageOrderViewModel ReturnViewModel(string searchString,string filterValue,int pageNum,List<Order> orders)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            viewModel.SearchString = searchString;
            viewModel.FilterValue = filterValue;
            viewModel.FilterValueText = "";
            viewModel.Orders = orders;
            viewModel.HasPreviousPages = false;
            viewModel.CurrentPage = pageNum;
            viewModel.PageNumber = pageNum;
            viewModel.HasNextPages = false;
            viewModel.Pages = Enumerable.Range(1, 1).ToArray();

            return viewModel;
        }
        [Fact]
       
        public async Task Index_WhenUserFirstClicks_ReturnAllOrders()
        {
            var orderDetailMockRepo = new Mock<IOrderDetailRepository>();
            var orderMockRepo = new Mock<IOrderRepository>();
            var orderStatusMockRepo = new Mock<IOrderStatusRepository>();
            var emailSenderMockRepo = new Mock<INotifications>();

            var viewModel = ReturnViewModel("", "", 1, ReturnListOfOrders());
            orderMockRepo.Setup(r => r.GetAllOrders(1)).ReturnsAsync(viewModel);

            var controller = new OrdersController(orderDetailMockRepo.Object, orderMockRepo.Object, orderStatusMockRepo.Object, emailSenderMockRepo.Object);

            var result = await controller.Index("", "", "", 0);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ManageOrderViewModel>(viewResult.ViewData.Model);
            Assert.Equal("2", model.Orders.Count() + "");
            
        }
    }
}
