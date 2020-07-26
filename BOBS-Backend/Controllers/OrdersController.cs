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
using BOBS_Backend.Notifications.NotificationsInterface;
using System.Text.RegularExpressions;

namespace BOBS_Backend.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderDetailRepository _orderDetail;
        private IOrderRepository _order;
        private IOrderStatusRepository _orderStatus;
        private INotifications _emailSender;
   
        public OrdersController(IOrderDetailRepository orderDetail, IOrderRepository order, IOrderStatusRepository orderStatus, INotifications emailSender)
        {
            _orderDetail = orderDetail;
            _order = order;
            _orderStatus = orderStatus;
            _emailSender = emailSender;
        }
       
        public async Task<IActionResult> EditOrderDetailAsync(long orderId, long orderDetailId,string quantity,int maxQuant,bool isLast)
        {

            int quant;
            bool res;
            int status = 0;
            
            res = int.TryParse(quantity,out quant);

            string errorMessage = "";

            if (res)
            {
                if (quant > maxQuant)
                {
                    errorMessage = "Error Occurred: Quantity must be less or equal to the max quantity";
                    return RedirectToAction("ProcessOrders", new { orderId, errorMessage });
                }

                var emailInfo = await _orderDetail.MakeOrderDetailInactive(orderDetailId, orderId, quant);
                if (emailInfo != null)
                {
                    
                    if (isLast)
                    {
                        status = 5;
                        return RedirectToAction("UpdateOrderStatus", new { orderId, status });
                    }
                    else
                    {
                        _emailSender.SendItemRemovalEmail(emailInfo["bookName"], emailInfo["bookCondition"], emailInfo["customerFirstName"], emailInfo["customerEmail"]);
                    }
                }
                else
                {
                    errorMessage = "Error Occurred: When trying to remove item. Please try again";
                }

            }
            else
            {
                errorMessage = " Error Occurred: Quantity must be a integer";
            }

            return RedirectToAction("ProcessOrders", new {orderId, errorMessage });
        }
        public async Task<IActionResult> Index(string filterValue, string filterValueText, string searchString, int pageNum)
        {
            if (pageNum == 0) pageNum++;
       
            if ( (String.IsNullOrEmpty(searchString)  && String.IsNullOrEmpty(filterValue)) )
            {
                var orders = await _order.GetAllOrders(pageNum);

                return View(orders);
            }
            else if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(filterValue))
            {
                searchString = Regex.Replace(searchString, @"\s+", " ");

                var orders = await _order.FilterList(filterValue, searchString,pageNum);


                filterValueText = filterValueText.Replace("Filter By: ", "");
                orders.FilterValueText = filterValueText;

                return View(orders);
            }
            else
            {
                ManageOrderViewModel viewModel = new ManageOrderViewModel();

                int[] pages = Enumerable.Range(1, 1).ToArray();

                filterValueText = filterValueText.Replace("Filter By: ", "");

                viewModel.Orders = null;
                viewModel.FilterValue = filterValue;
                viewModel.SearchString = searchString;
                viewModel.FilterValueText = filterValueText;
                viewModel.Pages = pages;
                viewModel.HasPreviousPages = false;
                viewModel.CurrentPage = 1;
                viewModel.HasNextPages = false;
                return View(viewModel);
            }
        }

        public async Task<IActionResult> UpdateOrderStatusAsync(long orderId,long status)
        {
            var order = await _order.FindOrderById(orderId);
            string errorMessage = "";
            if(status == 5)
            {
                order = await _order.CancelOrder(orderId);
            }
            else
            {
                order = await _orderStatus.UpdateOrderStatus(order, status);
            }    
            
            if(order != null)
            {
                _emailSender.SendOrderStatusUpdateEmail(order.OrderStatus.Status, order.Order_Id, order.Customer.FirstName, order.Customer.Email);

            }
            else
            {
                errorMessage = "Error Occured: Couldn't updated order status please try again";
            }
    

            return RedirectToAction("ProcessOrders", new { orderId });
        }

        public IActionResult Error()
        {
            return View();
        }


        public async Task<IActionResult> ProcessOrders(long orderId, string errorMessage)
        {
 
            if(string.IsNullOrEmpty(orderId.ToString()))
            {
                return RedirectToAction("Error");
            }
            else
            {
            
                var orderStatus = await _orderStatus.GetOrderStatuses();

                var order = await _order.FindOrderById(orderId);


                var orderDetails = await _orderDetail.FindOrderDetailByOrderId(orderId);

                ProcessOrderViewModel viewModel = new ProcessOrderViewModel();

                PartialOrder fullOrder = new PartialOrder();

                fullOrder.Order = order;
                fullOrder.OrderDetails = orderDetails;
                fullOrder.itemsRemoved = await _orderDetail.FindOrderDetailsRemovedCountAsync(orderId);

                viewModel.Statuses = orderStatus;
                viewModel.FullOrder = fullOrder;
                viewModel.errorMessage = string.IsNullOrEmpty(errorMessage) ? "" : errorMessage;

                return View(viewModel);
            }
        }
    }
}
