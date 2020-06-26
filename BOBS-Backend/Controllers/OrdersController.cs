using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.Database;
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
        private DatabaseContext _context;
        public OrdersController(DatabaseContext context,IOrderDetailRepository orderDetail, IOrderRepository order)
        {
            _orderDetail = orderDetail;
            _order = order;
            _context = context;
        }
        
        public IActionResult Index(string filterValue, string searchString)
        {

       
            if ( (String.IsNullOrEmpty(searchString)  && String.IsNullOrEmpty(filterValue)) || filterValue.Contains("All Orders"))
            {
                var orders = _order.GetAllOrders();

                ViewData["AllOrders"] = orders;

                return View();
            }
            else if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(filterValue))
            {

                var orders = _order.FilterList(filterValue, searchString);

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

        public IActionResult ProcessOrders(long orderId)
        {
            if(string.IsNullOrEmpty(orderId.ToString()))
            {
                return RedirectToAction("Error");
            }
            else
            {
                var order = _order.FindOrderById(orderId);

                var orderDetails = _orderDetail.FindOrderDetailByOrderId(orderId);

                PartialOrder fullOrder = new PartialOrder();

                fullOrder.Order = order;
                fullOrder.OrderDetails = orderDetails;

                ViewData["FullOrder"] = fullOrder;

                return View();
            }




        }
    }
}
