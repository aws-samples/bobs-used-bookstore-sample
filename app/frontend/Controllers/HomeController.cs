using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.AspNetCore.Identity.Cognito;
using BobsBookstore.Models;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.Models.Carts;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.Models.Customers;

namespace BobBookstore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<CognitoUser> _SignInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<CartItem> _cartItemRepository;

        private readonly IGenericRepository<Customer> _customerRepository;


        public HomeController(IGenericRepository<CartItem> cartItemRepository, IGenericRepository<Customer> customerRepository, IGenericRepository<Cart> cartRepository, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor,ApplicationDbContext context, SignInManager<CognitoUser> SignInManager, UserManager<CognitoUser> userManager)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _SignInManager = SignInManager;
            _userManager = userManager;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {

            
            string ip ;
            if (_SignInManager.IsSignedIn(User))
            {
                
                //get customer
                var user = await _userManager.GetUserAsync(User);
                var currentCustomer = _customerRepository.Get(user.Attributes[CognitoAttribute.Sub.AttributeName]);
                //get cart
                var cart = _cartRepository.Get(s => s.Customer == currentCustomer).FirstOrDefault();
                
                //put cartid in cookie
                if (!HttpContext.Request.Cookies.ContainsKey("CartId"))
                {
                    CookieOptions options = new CookieOptions();

                    HttpContext.Response.Cookies.Append("CartId", Convert.ToString(cart.Cart_Id));
                }
                else
                {

                    HttpContext.Response.Cookies.Delete("CartId");
                    HttpContext.Response.Cookies.Append("CartId", Convert.ToString(cart.Cart_Id));
                }
                //put cart item in user cart
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
                    CookieOptions options = new CookieOptions();
                    Guid gu_id = Guid.NewGuid();
                    ip = gu_id.ToString();
                    HttpContext.Response.Cookies.Append("CartIp", gu_id.ToString());
                    Guid gu_id1 = Guid.NewGuid();
                    string id = gu_id1.ToString();
                    var IP = new Cart() { IP = ip ,Cart_Id=id};
                    _cartRepository.Add(IP);
                    _cartRepository.Save();

                }
                else
                {
                    ip = HttpContext.Request.Cookies["CartIp"];
                }
                //put cart id in cookie
                var currentCart = _cartRepository.Get(s => s.IP == ip).FirstOrDefault();
               
              

                if (!HttpContext.Request.Cookies.ContainsKey("CartId"))
                {
                    CookieOptions options = new CookieOptions();
                    HttpContext.Response.Cookies.Append("CartId", Convert.ToString(currentCart.Cart_Id));

                }
                else
                {
                    HttpContext.Response.Cookies.Delete("CartId");
                    HttpContext.Response.Cookies.Append("CartId", Convert.ToString(currentCart.Cart_Id));
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
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
