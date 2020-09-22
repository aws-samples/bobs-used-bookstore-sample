using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using BobBookstore.Data;
using BobBookstore.Models.Order;
using BobBookstore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BobBookstore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly UsedBooksContext _context;
        private readonly SignInManager<CognitoUser> _SignInManager;
        private readonly UserManager<CognitoUser> _userManager;

        public OrdersController(UsedBooksContext context, SignInManager<CognitoUser> SignInManager, UserManager<CognitoUser> userManager)
        {
            _context = context;
            _SignInManager = SignInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if (_SignInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                string customer_id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                var orders = from o in _context.Order
                             where o.Customer.Customer_Id == customer_id
                             select new OrderViewModel()
                             {
                                 Order_Id = o.Order_Id,
                                 Status = o.OrderStatus.Status,
                                 Tax = o.Tax,
                                 Subtotal = o.Subtotal,
                                 DeliveryDate = o.DeliveryDate
                             };


                return View(await orders.ToListAsync());
            }
            return NotFound("You must be signed in.");
        }

        public async Task<IActionResult> Detail(long id)
        {
            if (_SignInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                string customer_id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                var orderDetails = from od in _context.OrderDetail
                                   where od.Order.Order_Id == id && !od.IsRemoved
                                   select new OrderDetailViewModel()
                                   {
                                       Bookname = od.Book.Name,
                                       Book_Id = od.Book.Book_Id,
                                       quantity = od.quantity,
                                       price = od.price,
                                       Url=od.Book.Back_Url
                                   };
                var books = await orderDetails.OrderBy(o => o.Bookname).ToListAsync();

                var order = from o in _context.Order
                            where o.Customer.Customer_Id == customer_id &&
                            o.Order_Id == id
                            select new OrderViewModel()
                            {
                                Order_Id = o.Order_Id,
                                Status = o.OrderStatus.Status,
                                Tax = o.Tax,
                                Subtotal = o.Subtotal,
                                DeliveryDate = o.DeliveryDate,
                                Books = books
                            };
                return View(order.FirstOrDefault());

            }
            return NotFound("You must be signed in.");
        }

        public async Task<IActionResult> Delete(long id)
        {
            // needs rework, buggy rn
            //TODO Create new order_status (cancel), reassign to order
            var order = (from o in _context.Order where o.Order_Id == id select o).First();

            if (order == null)
            {
                return NotFound();
            }

            var cancel_status = (from o in _context.OrderStatus where o.Status.Equals("Cancelled") select o).First();

            if (cancel_status == null)
            {
                // create it
                cancel_status = new OrderStatus
                {
                    Status = "Cancelled"
                };
                _context.OrderStatus.Add(cancel_status);
            }

            order.OrderStatus = cancel_status;
            _context.Order.Update(order);

            await _context.SaveChangesAsync();

            return View();
        }
    }
}
