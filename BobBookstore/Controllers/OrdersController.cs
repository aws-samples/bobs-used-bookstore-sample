using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobBookstore.Data;
using BobBookstore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BobBookstore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly UsedBooksContext _context;

        public OrdersController(UsedBooksContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync(string id)
        {
            var orders = from o in _context.Order where o.Customer.Customer_Id == id
                         select new OrderViewModel() 
                         {
                             Status = o.OrderStatus.Status,
                             Tax = o.Tax,
                             Subtotal = o.Subtotal,
                             DeliveryDate = o.DeliveryDate
                         };


            return View(await orders.ToListAsync());
        }
<<<<<<< HEAD

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
=======
>>>>>>> ca6445f9ff3b47384a19a76d3247e9e74acaafb9
    }
}
