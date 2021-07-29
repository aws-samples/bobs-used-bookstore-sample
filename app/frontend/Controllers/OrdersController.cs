using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.Models.Orders;
using BobsBookstore.Models.ViewModels;
using BookstoreFrontend.Models.ViewModels;
using System.Collections.Generic;
using BobsBookstore.DataAccess.Repository.Interface;
/*using BookstoreBackend.Database;
*/
namespace BobBookstore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<CognitoUser> _SignInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;



        public OrdersController(IGenericRepository<Order> orderRepository, IGenericRepository<OrderDetail> orderDetailRepository, ApplicationDbContext context, SignInManager<CognitoUser> SignInManager, UserManager<CognitoUser> userManager)
        {
            _context = context;
            _SignInManager = SignInManager;
            _userManager = userManager;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<IActionResult> Index()
        {
            if (_SignInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                string customer_id = user.Attributes[CognitoAttribute.Sub.AttributeName];

                IEnumerable<Order> orders = _orderRepository.Get(o => o.Customer.Customer_Id == customer_id, includeProperties:"OrderStatus,Customer");

                return View(orders);
            }
            return NotFound("You must be signed in.");
        }

        public async Task<IActionResult> Detail(long id)
        {
            if (_SignInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                string customer_id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                IEnumerable<OrderDetail> orderDetails = _orderDetailRepository.Get(od => od.Order.Order_Id == id && !od.IsRemoved, orderBy: q => q.OrderBy(s => s.Book.Name), includeProperties: "Book");
                IEnumerable<Order> order = _orderRepository.Get(o => o.Customer.Customer_Id == customer_id && o.Order_Id == id, includeProperties: "OrderStatus");
                
                OrderDisplayModel viewModel = new OrderDisplayModel();
                viewModel.OrderBookDetails = orderDetails;
                viewModel.OrderDetail = order.FirstOrDefault();

                return View(viewModel);

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
