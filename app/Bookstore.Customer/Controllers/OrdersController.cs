using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using CustomerSite.Models.ViewModels;
using Bookstore.Domain.Orders;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface;

namespace CustomerSite.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;

        public OrdersController(IGenericRepository<Order> orderRepository,
                                IGenericRepository<OrderDetail> orderDetailRepository,
                                ApplicationDbContext context,
                                SignInManager<CognitoUser> signInManager,
                                UserManager<CognitoUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var customerId = user.Attributes[CognitoAttribute.Sub.AttributeName];

                var orders = _orderRepository.Get(o => o.Customer.Customer_Id == customerId,
                    includeProperties: "OrderStatus,Customer");

                return View(orders);
            }

            return NotFound("You must be signed in.");
        }

        public async Task<IActionResult> Detail(long id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var customer_id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                var orderDetails = _orderDetailRepository.Get(od => od.Order.Order_Id == id && !od.IsRemoved,
                    q => q.OrderBy(s => s.Book.Name), "Book");
                var order = _orderRepository.Get(o => o.Customer.Customer_Id == customer_id && o.Order_Id == id,
                    includeProperties: "OrderStatus");

                var viewModel = new OrderDisplayModel
                {
                    OrderBookDetails = orderDetails,
                    OrderDetail = order.FirstOrDefault()
                };

                return View(viewModel);
            }

            return NotFound("You must be signed in.");
        }

        public async Task<IActionResult> Delete(long id)
        {
            // needs rework, buggy rn
            //TODO Create new order_status (cancel), reassign to order
            var order = (from o in _context.Order where o.Order_Id == id select o).First();

            if (order == null) return NotFound();

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
