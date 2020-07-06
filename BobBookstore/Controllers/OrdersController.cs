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

        public async Task<IActionResult> IndexAsync(long customer)
        {
            var orders = from o in _context.Order where o.Customer.Customer_Id == customer
                         select new OrderViewModel()
                         {
                             Subtotal = o.Subtotal,
                             Tax = o.Tax,
                             DeliveryDate = o.DeliveryDate,
                             Status = o.OrderStatus.Status
                         };

            return View(await orders.ToListAsync());
        }
    }
}
