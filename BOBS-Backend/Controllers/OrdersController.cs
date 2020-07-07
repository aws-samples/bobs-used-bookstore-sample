using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository;
using BOBS_Backend.Repository.OrdersInterface;
using BOBS_Backend.ViewModel.ManageOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BOBS_Backend.ViewModel.ProcessOrders;
using System.Runtime.InteropServices;

namespace BOBS_Backend.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderDetailRepository _orderDetail;
        private IOrderRepository _order;
        private IOrderStatusRepository _orderStatus;
   
        public OrdersController(IOrderDetailRepository orderDetail, IOrderRepository order, IOrderStatusRepository orderStatus)
        {
            _orderDetail = orderDetail;
            _order = order;
            _orderStatus = orderStatus;
        }
       
        public async Task<IActionResult> Index(string filterValue, string searchString, int pageNum)
        {
            if (pageNum == 0) pageNum++;
       
            if ( (String.IsNullOrEmpty(searchString)  && String.IsNullOrEmpty(filterValue)) || filterValue.Contains("All Orders"))
            {
                var orders = await _order.GetAllOrders(pageNum);

                return View(orders);
            }
            else if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(filterValue))
            {

                var orders = await _order.FilterList(filterValue, searchString,pageNum);

                return View(orders);
            }
            else
            {
                ManageOrderViewModel viewModel = new ManageOrderViewModel();

                int[] pages = Enumerable.Range(1, 1).ToArray();

                viewModel.Orders = null;
                viewModel.FilterValue = filterValue;
                viewModel.SearchString = searchString;
                viewModel.Pages = pages;
                viewModel.HasPreviousPages = false;
                viewModel.CurrentPage = 1;
                viewModel.HasNextPages = false;
                return View(viewModel);
            }
        }


        public IActionResult Error()
        {
            return View();
        }


        public async Task<IActionResult> ProcessOrders(long orderId, long status)
        {
 
            if(string.IsNullOrEmpty(orderId.ToString()))
            {
                return RedirectToAction("Error");
            }
            else
            {
            
                var orderStatus = await _orderStatus.GetOrderStatuses();

                var order = await _order.FindOrderById(orderId);

                if(status != 0)
                {
                    order = await _orderStatus.UpdateOrderStatus(order, status);
                }

                var orderDetails = await _orderDetail.FindOrderDetailByOrderId(orderId);

                ProcessOrderViewModel viewModel = new ProcessOrderViewModel();

                PartialOrder fullOrder = new PartialOrder();

                fullOrder.Order = order;
                fullOrder.OrderDetails = orderDetails;

                viewModel.Statuses = orderStatus;
                viewModel.FullOrder = fullOrder;

                return View(viewModel);
            }
        }
    }
}
