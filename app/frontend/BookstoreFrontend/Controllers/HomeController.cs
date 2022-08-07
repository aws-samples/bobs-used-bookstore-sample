using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.Models.Carts;
using BobsBookstore.Models.Customers;
using BookstoreFrontend.Models;

namespace BookstoreFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenericRepository<CartItem> _cartItemRepository;
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly ApplicationDbContext _context;

        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;

        public HomeController(IGenericRepository<CartItem> cartItemRepository,
                              IGenericRepository<Customer> customerRepository,
                              IGenericRepository<Cart> cartRepository,
                              ILogger<HomeController> logger,
                              IHttpContextAccessor httpContextAccessor,
                              ApplicationDbContext context,
                              SignInManager<CognitoUser> signInManager,
                              UserManager<CognitoUser> userManager)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            string ip;
            if (_signInManager.IsSignedIn(User))
            {
                // get customer
                var user = await _userManager.GetUserAsync(User);
                var currentCustomer = _customerRepository.Get(user.Attributes[CognitoAttribute.Sub.AttributeName]);
                // get cart
                var cart = _cartRepository.Get(s => s.Customer == currentCustomer).FirstOrDefault();

                // put cartid in cookie
                if (!HttpContext.Request.Cookies.ContainsKey("CartId"))
                {
                    HttpContext.Response.Cookies.Append("CartId", cart.Cart_Id);
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("CartId");
                    HttpContext.Response.Cookies.Append("CartId", cart.Cart_Id);
                }

                // put cart item in user cart
                var cartItem = _cartItemRepository.Get(c => c.Cart == cart && c.WantToBuy == true);

                foreach (var item in cartItem)
                {
                    item.Cart = cart;
                    _cartItemRepository.Update(item);
                }

                _cartItemRepository.Save();
            }
            else
            {
                if (!HttpContext.Request.Cookies.ContainsKey("CartIp"))
                {
                    ip = Guid.NewGuid().ToString();
                    HttpContext.Response.Cookies.Append("CartIp", ip);
                    var id = Guid.NewGuid().ToString();
                    var cartInfo = new Cart { IP = ip, Cart_Id = id };
                    _cartRepository.Add(cartInfo);
                    _cartRepository.Save();
                }
                else
                {
                    ip = HttpContext.Request.Cookies["CartIp"];
                }

                // put cart id in cookie
                var currentCart = _cartRepository.Get(s => s.IP == ip).FirstOrDefault();
                if (currentCart == null)
                {
                    var id = Guid.NewGuid().ToString();
                    currentCart = new Cart { IP = ip, Cart_Id = id };
                    _cartRepository.Add(currentCart);
                    _cartRepository.Save();
                }

                if (!HttpContext.Request.Cookies.ContainsKey("CartId"))
                {
                    HttpContext.Response.Cookies.Append("CartId", currentCart.Cart_Id);
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("CartId");
                    HttpContext.Response.Cookies.Append("CartId", currentCart.Cart_Id);
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search()
        {
            return RedirectToAction("Index", "Search");
        }

        public IActionResult Cart()
        {
            return RedirectToAction("Index", "CartItems");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
