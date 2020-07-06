using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository;
using BOBS_Backend.ViewModel.ManageOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BOBS_Backend.Controllers
{
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
        
        public async Task<IActionResult> Index(string filterValue, string searchString)
        {

       
            if ( (String.IsNullOrEmpty(searchString)  && String.IsNullOrEmpty(filterValue)) || filterValue.Contains("All Orders"))
            {
                var orders = await _order.GetAllOrders();

                ViewData["AllOrders"] = orders;

                return View();
            }
            else if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(filterValue))
            {

                var orders = await _order.FilterList(filterValue, searchString);

                ViewData["AllOrders"] = orders;
                return View();
            }
            else
            {

                return View();
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

                PartialOrder fullOrder = new PartialOrder();

                fullOrder.Order = order;
                fullOrder.OrderDetails = orderDetails;

                ViewData["FullOrder"] = fullOrder;

                ViewData["OrderStatus"] = orderStatus;

                return View();
            }




        }
    }
}
