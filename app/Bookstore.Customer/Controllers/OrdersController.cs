using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Domain.Orders;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface;
using Bookstore.Customer;
using Bookstore.Customer.ViewModel;

namespace CustomerSite.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
        private readonly IGenericRepository<Order> _orderRepository;
        public OrdersController(IGenericRepository<Order> orderRepository,
                                IGenericRepository<OrderDetail> orderDetailRepository,
                                ApplicationDbContext context)
        {
            _context = context;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //var customerId = user.Attributes[CognitoAttribute.Sub.AttributeName];

                var orders = _orderRepository.Get(o => o.Customer.Sub == User.GetUserId(),
                    includeProperties: "Customer");

                return View(orders);
            }

            return NotFound("You must be signed in.");
        }

        public async Task<IActionResult> Detail(long id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var orderDetails = _orderDetailRepository.Get(od => od.Order.Id == id && !od.IsRemoved,
                    q => q.OrderBy(s => s.Book.Name), "Book");
                var order = _orderRepository.Get(o => o.Customer.Sub == User.GetUserId() && o.Id == id);

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
            var order = (from o in _context.Order where o.Id == id select o).First();

            if (order == null) return NotFound();

            order.OrderStatus = OrderStatus.Cancelled;
            _context.Order.Update(order);

            await _context.SaveChangesAsync();

            return View();
        }
    }
}
