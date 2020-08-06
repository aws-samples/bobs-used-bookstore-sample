using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BobBookstore.Models;
using Microsoft.AspNetCore.Http;
using BobBookstore.Data;
using BobBookstore.Models.Carts;
using Microsoft.AspNetCore.Identity;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.AspNetCore.Identity.Cognito;
using BobBookstore.Models.Customer;
using BobBookstore.Models.Book;

namespace BobBookstore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsedBooksContext _context;
        private readonly SignInManager<CognitoUser> _SignInManager;
        private readonly UserManager<CognitoUser> _userManager;
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor,UsedBooksContext context, SignInManager<CognitoUser> SignInManager, UserManager<CognitoUser> userManager)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _SignInManager = SignInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            
            string ip ;
            if (_SignInManager.IsSignedIn(User))
            {
                
                //get customer
                var user = await _userManager.GetUserAsync(User);
                var currentCustomer = _context.Customer.Find(user.Attributes[CognitoAttribute.Sub.AttributeName]);

                //get cart
                var cart = from c in _context.Cart
                           select c;
                cart = cart.Where(s => s.Customer == currentCustomer);
                Cart currentCart = new Cart();
                foreach (var ca in cart)
                {
                    currentCart = ca;
                }
                //put cartid in cookie
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
                //put cart item in user cart
                var id = Convert.ToString (HttpContext.Request.Cookies["CartId"]);
                var cartC = _context.Cart.Find(id);
                var cartItem = from c in _context.CartItem
                               where c.Cart == cartC && c.WantToBuy == true
                               select c;
                var userC = await _userManager.GetUserAsync(User);

                var UserId = userC.Attributes[CognitoAttribute.Sub.AttributeName];

                var recentcustomer = _context.Customer.Find(UserId);
                var customerCart = from c in _context.Cart
                                   where c.Customer == recentcustomer
                                   select c;
                Cart recentCart = new Cart();
                foreach (var item in customerCart)
                {
                    recentCart = item;
                }
                //foreach (var item in cartItem)
                //{
                //    item.Cart = recentCart;
                //    _context.Update(item);
                //}
                await _context.SaveChangesAsync();
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
                    _context.Add(IP);

                    await _context.SaveChangesAsync();

                }
                else
                {
                    string CartIp = HttpContext.Request.Cookies["CartIp"];
                    ip = CartIp;
                }
                //put cart id in cookie
                var cart = from c in _context.Cart
                           select c;
                cart = cart.Where(s => s.IP == ip);
                Cart currentCart = new Cart();
                foreach (var ca in cart)
                {
                    currentCart = ca;
                }

                if (!HttpContext.Request.Cookies.ContainsKey("CartId"))
                {
                    CookieOptions options = new CookieOptions();
                    HttpContext.Response.Cookies.Append("CartId",Convert.ToString(currentCart.Cart_Id));

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
