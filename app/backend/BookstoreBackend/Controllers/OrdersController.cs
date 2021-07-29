using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BobsBookstore.DataAccess.Repository.Interface.OrdersInterface;
using BookstoreBackend.ViewModel.ManageOrders;
using BookstoreBackend.ViewModel.ProcessOrders;
using BookstoreBackend.Notifications.NotificationsInterface;
using BobsBookstore.DataAccess.Dtos;
using AutoMapper;

namespace BookstoreBackend.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderDetailRepository _orderDetail;
        private IOrderRepository _order;
        private IOrderStatusRepository _orderStatus;
        private INotifications _emailSender;
        private readonly IMapper _mapper;



        public OrdersController() { }

        [ActivatorUtilitiesConstructor]
        public OrdersController(IMapper mapper, IOrderDetailRepository orderDetail, IOrderRepository order, IOrderStatusRepository orderStatus, INotifications emailSender)
        {
            _orderDetail = orderDetail;
            _order = order;
            _orderStatus = orderStatus;
            _emailSender = emailSender;
            _mapper = mapper;
        }
       
        public async Task<IActionResult> EditOrderDetailAsync(long orderId, long orderDetailId,string quantity,int maxQuant,bool isLast)
        {

            int quant;
            bool res;
            long status = 0;
            
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
                        var cancelled = _orderStatus.FindOrderStatusByName("Cancelled");
                        var order = _order.FindOrderById(orderId);
                        status = cancelled.OrderStatus_Id;
                        return RedirectToAction("UpdateOrderStatus", new { orderId, status, oldStatus = order.OrderStatus.OrderStatus_Id });
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
        public IActionResult Index(string filterValue, string filterValueText, string searchString, int pageNum)
        {
            if (pageNum == 0) pageNum++;

            if ((String.IsNullOrEmpty(searchString) && String.IsNullOrEmpty(filterValue)))
            {
                var orders = _order.GetAllOrders(pageNum);
                var manageOrderViewModel = _mapper.Map<ManageOrderViewModel>(orders);
                return View(manageOrderViewModel);
            }
            else if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(filterValue))
            {
                searchString = Regex.Replace(searchString, @"\s+", " ");

                var orders = _order.FilterList(filterValue, searchString, pageNum);


                filterValueText = filterValueText.Replace("Filter By: ", "");
                orders.FilterValueText = filterValueText;

                return View(orders);
            }
            else
            {
                ManageOrderViewModel manageOrderViewModel = new ManageOrderViewModel();

                int[] pages = Enumerable.Range(1, 1).ToArray();

                filterValueText = filterValueText.Replace("Filter By: ", "");

                manageOrderViewModel.Orders = null;
                manageOrderViewModel.FilterValue = filterValue;
                manageOrderViewModel.SearchString = searchString;
                manageOrderViewModel.FilterValueText = filterValueText;
                manageOrderViewModel.Pages = pages;
                manageOrderViewModel.HasPreviousPages = false;
                manageOrderViewModel.CurrentPage = 1;
                manageOrderViewModel.HasNextPages = false;
                return View(manageOrderViewModel);
            }
        }

        public async Task<IActionResult> UpdateOrderStatusAsync(long orderId,long status, long oldStatus)
        {
            var order = _order.FindOrderById(orderId);
            string errorMessage = "";
            var cancelled = _orderStatus.FindOrderStatusByName("Cancelled");
            var newStatus = _orderStatus.FindOrderStatusById(status);

            if (newStatus.position < order.OrderStatus.position || order.OrderStatus.OrderStatus_Id != oldStatus)
            {
                errorMessage = "Error Occurred: The order status has changed";
                return RedirectToAction("ProcessOrders", new { orderId, errorMessage });
            }

            if (status == order.OrderStatus.OrderStatus_Id)
            {
                errorMessage = "Error Occurred: This is already the order status of the order";
                return RedirectToAction("ProcessOrders", new { orderId, errorMessage });
            }
            
           
            if(status == cancelled.OrderStatus_Id)
            {
                order = await _orderDetail.CancelOrder(orderId);
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
    

            return RedirectToAction("ProcessOrders", new { orderId, errorMessage });
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
            
                var orderStatus = _orderStatus.GetOrderStatuses();

                var order = _order.FindOrderById(orderId);


                var orderDetails = await _orderDetail.FindOrderDetailByOrderId(orderId);

                ProcessOrderViewModel viewModel = new ProcessOrderViewModel();

                PartialOrderViewModel fullOrder = new PartialOrderViewModel();

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
