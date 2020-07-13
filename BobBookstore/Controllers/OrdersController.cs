using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobBookstore.Data;
using BobBookstore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
                             Order_Id = o.Order_Id,
                             Status = o.OrderStatus.Status,
                             Tax = o.Tax,
                             Subtotal = o.Subtotal,
                             DeliveryDate = o.DeliveryDate
                         };


            return View(await orders.ToListAsync());
        }

        public async Task<IActionResult> DeleteAsync(long id)
        {
            var order = (from o in _context.Order where o.Order_Id == id select o)
                .FirstOrDefault();
            var status = (from s in _context.OrderStatus
                          where order.OrderStatus.OrderStatus_Id == s.OrderStatus_Id
                          select s).FirstOrDefault();

            if (status == null)
            {
                return NotFound();
            }

            status.Status = "Cancelled";
            _context.Update(status);
            await _context.SaveChangesAsync();

            return View();
        }
    }
}
