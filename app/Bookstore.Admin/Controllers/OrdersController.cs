using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using AdminSite.ViewModel.ManageOrders;
using AdminSite.ViewModel.ProcessOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Bookstore.Data.Repository.Interface.OrdersInterface;
using Bookstore.Data.Repository.Interface.NotificationsInterface;
using Bookstore.Admin;

namespace AdminSite.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly INotifications _emailSender;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _order;
        private readonly IOrderDetailRepository _orderDetail;
        private readonly IOrderStatusRepository _orderStatus;

        public OrdersController()
        {
        }

        [ActivatorUtilitiesConstructor]
        public OrdersController(IMapper mapper,
            IOrderDetailRepository orderDetail,
            IOrderRepository order,
            IOrderStatusRepository orderStatus,
            INotifications emailSender)
        {
            _orderDetail = orderDetail;
            _order = order;
            _orderStatus = orderStatus;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        public async Task<IActionResult> EditOrderDetailAsync(long orderId, long orderDetailId, string quantity,
            int maxQuant, bool isLast)
        {
            var res = int.TryParse(quantity, out var quant);

            var errorMessage = "";

            if (res)
            {
                if (quant > maxQuant)
                {
                    errorMessage = Resource.ResourceManager.GetString("MaxQuantityErrorMessage");
                    return RedirectToAction("ProcessOrders", new { orderId, errorMessage });
                }

                var emailInfo = await _orderDetail.MakeOrderDetailInactive(orderDetailId, orderId, quant);
                if (emailInfo != null)
                {
                    if (isLast)
                    {
                        var cancelled = _orderStatus.FindOrderStatusByName("Cancelled");
                        var order = _order.FindOrderById(orderId);
                        var status = cancelled.OrderStatus_Id;
                        return RedirectToAction("UpdateOrderStatus",
                            new { orderId, status, oldStatus = order.OrderStatus.OrderStatus_Id });
                    }

                    _emailSender.SendItemRemovalEmail(emailInfo["bookName"], emailInfo["bookCondition"],
                        emailInfo["customerFirstName"], emailInfo["customerEmail"]);
                }
                else
                {
                    errorMessage = Resource.ResourceManager.GetString("RemoveFailErrorMessage");
                }
            }
            else
            {
                errorMessage = Resource.ResourceManager.GetString("IntegerQuantityErrorMessage");
            }

            return RedirectToAction("ProcessOrders", new { orderId, errorMessage });
        }

        public IActionResult Index(string filterValue, string filterValueText, string searchString, int pageNum)
        {
            if (pageNum == 0) pageNum++;

            //if (string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(filterValue))
            //{
            //    var orders = _order.GetAllOrders(pageNum);
            //    var manageOrderViewModel = _mapper.Map<ManageOrderViewModel>(orders);
            //    return View(manageOrderViewModel);
            //}

            //if (!string.IsNullOrEmpty(searchString) && !string.IsNullOrEmpty(filterValue))
            //{
            //    searchString = Regex.Replace(searchString, @"\s+", " ");

            //    var orders = _order.FilterList(filterValue, searchString, pageNum);

            //    filterValueText = filterValueText.Replace("Filter By: ", "");
            //    orders.FilterValueText = filterValueText;
            //    var manageOrderViewModel = _mapper.Map<ManageOrderViewModel>(orders);
            //    return View(manageOrderViewModel);
            //}
            //else
            //{
            //    var manageOrderViewModel = new ManageOrderViewModel();

            //    var pages = Enumerable.Range(1, 1).ToArray();

            //    filterValueText = filterValueText.Replace("Filter By: ", "");

            //    manageOrderViewModel.Orders = null;
            //    manageOrderViewModel.FilterValue = filterValue;
            //    manageOrderViewModel.SearchString = searchString;
            //    manageOrderViewModel.FilterValueText = filterValueText;
            //    manageOrderViewModel.Pages = pages;
            //    manageOrderViewModel.HasPreviousPages = false;
            //    manageOrderViewModel.CurrentPage = 1;
            //    manageOrderViewModel.HasNextPages = false;
            //    return View(manageOrderViewModel);
            //}

            return View();
        }

        public async Task<IActionResult> UpdateOrderStatusAsync(long orderId, long status, long oldStatus)
        {
            var order = _order.FindOrderById(orderId);
            var errorMessage = "";
            var cancelled = _orderStatus.FindOrderStatusByName("Cancelled");
            var newStatus = _orderStatus.FindOrderStatusById(status);

            if (newStatus.Position < order.OrderStatus.Position || order.OrderStatus.OrderStatus_Id != oldStatus)
            {
                errorMessage = Resource.ResourceManager.GetString("OrderStatusChangeErrorMessage");
                return RedirectToAction("ProcessOrders", new { orderId, errorMessage });
            }

            if (status == order.OrderStatus.OrderStatus_Id)
            {
                errorMessage = Resource.ResourceManager.GetString("OrderStatusNoChangeErrorMessage");
                return RedirectToAction("ProcessOrders", new { orderId, errorMessage });
            }


            if (cancelled !=null && status == cancelled.OrderStatus_Id)
                order = await _orderDetail.CancelOrder(orderId);
            else
                order = await _orderStatus.UpdateOrderStatus(order, status);

            if (order != null)
                _emailSender.SendOrderStatusUpdateEmail(order.OrderStatus.Status, order.Order_Id,
                    order.Customer.FirstName, order.Customer.Email);
            else
                errorMessage = Resource.ResourceManager.GetString("OrderStatusUpdateErrorMessage");

            return RedirectToAction("ProcessOrders", new { orderId, errorMessage });
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> ProcessOrdersAsync(long orderId, string errorMessage)
        {
            if (string.IsNullOrEmpty(orderId.ToString()))
                return RedirectToAction("Error");

            var orderStatus = _orderStatus.GetOrderStatuses();
            var order = _order.FindOrderById(orderId);
            var orderDetails = await _orderDetail.FindOrderDetailByOrderIdAsync(orderId);

            var fullOrder = new PartialOrderViewModel
            {
                Order = order,
                OrderDetails = orderDetails,
                ItemsRemoved = await _orderDetail.FindOrderDetailsRemovedCountAsync(orderId)
            };

            var viewModel = new ProcessOrderViewModel
            {
                Statuses = orderStatus,
                FullOrder = fullOrder,
                ErrorMessage = string.IsNullOrEmpty(errorMessage) ? "" : errorMessage
            };

            return View(viewModel);
        }
    }
}
