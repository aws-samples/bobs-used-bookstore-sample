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

            //Add Guid to session coookie
            //HttpContext.Response.Cookies.Delete("CartIp");
            string ip = "0";
            if (_SignInManager.IsSignedIn(User))
            {
                

                var user = await _userManager.GetUserAsync(User);
                var email = user.Attributes[CognitoAttribute.Email.AttributeName];
                var customer = from m in _context.Customer
                               select m;
                customer = customer.Where(s => s.Email == email);


                Customer currentCustomer = new Customer();
                foreach (var cus in customer)
                {
                    currentCustomer = cus;
                }

                var cart = from c in _context.Cart
                           select c;
                cart = cart.Where(s => s.Customer == currentCustomer);
                Cart currentCart = new Cart();
                foreach (var ca in cart)
                {
                    currentCart = ca;
                }

                if (!HttpContext.Request.Cookies.ContainsKey("CartId"))
                {
                    CookieOptions options = new CookieOptions();

                    HttpContext.Response.Cookies.Append("CartId", Convert.ToString(currentCart.Cart_Id));


                }
                else
                {
                    

                        var id = Convert.ToInt32(HttpContext.Request.Cookies["CartId"]);
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
                        foreach (var item in cartItem)
                        {
                            item.Cart = recentCart;
                            _context.Update(item);
                        }
                        await _context.SaveChangesAsync();
                    
                    HttpContext.Response.Cookies.Delete("CartId");
                    HttpContext.Response.Cookies.Append("CartId", Convert.ToString(currentCart.Cart_Id));
                }
            }
            else
            {
                if (!HttpContext.Request.Cookies.ContainsKey("CartIp"))
                {
                    CookieOptions options = new CookieOptions();
                    Guid gu_id = Guid.NewGuid();
                    ip = gu_id.ToString();
                    HttpContext.Response.Cookies.Append("CartIp", gu_id.ToString());
                    var IP = new Cart() { IP = ip };
                    _context.Add(IP);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    string CartIp = HttpContext.Request.Cookies["CartIp"];
                    ip = CartIp;
                }
                var cart = from c in _context.Cart
                           select c;
                cart = cart.Where(s => s.IP == HttpContext.Request.Cookies["CartIp"]);
                Cart currentCart = new Cart();
                foreach (var ca in cart)
                {
                    currentCart = ca;
                }

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
