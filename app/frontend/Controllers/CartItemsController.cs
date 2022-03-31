using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.AspNetCore.Identity.Cognito;
using BobsBookstore.Models.Orders;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.Models.Carts;
using BobsBookstore.Models.ViewModels;
using BobsBookstore.Models.Customers;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.Models.Books;

namespace BobBookstore.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<CognitoUser> _SignInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly IGenericRepository<CartItem> _cartItemRepository;
        private readonly IGenericRepository<OrderStatus> _orderStatusRepository;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<Price> _priceRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Address> _addressRepository;

        public CartItemsController(IGenericRepository<Address> addressRepository, IGenericRepository<OrderDetail> orderDetailRepository, IGenericRepository<Order> orderRepository, IGenericRepository<Price> priceRepository, IGenericRepository<Book> bookRepository, IGenericRepository<Customer> customerRepository, IGenericRepository<OrderStatus> orderStatusRepository, IGenericRepository<CartItem> cartItemRepository, ApplicationDbContext context, SignInManager<CognitoUser> SignInManager, UserManager<CognitoUser> userManager)
        {
            _context = context;
            _SignInManager = SignInManager;
            _userManager = userManager;
            _cartItemRepository = cartItemRepository;
            _orderStatusRepository = orderStatusRepository;
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
            _priceRepository = priceRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _addressRepository = addressRepository;

        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            
            var id = Convert.ToString (HttpContext.Request.Cookies["CartId"]);

            IEnumerable<CartViewModel> cartItem = _cartItemRepository.Get(c => c.Cart.Cart_Id == id && c.WantToBuy == true, includeProperties: "Price,Book,Cart")
                .Select(c => new CartViewModel
                {
                    BookId = c.Book.Book_Id,
                    Url = c.Book.FrontUrl,
                    Prices = c.Price.ItemPrice,
                    BookName = c.Book.Name,
                    CartItem_Id = c.CartItem_Id,
                    Quantity = c.Price.Quantity,
                    PriceId = c.Price.Price_Id,

                });
         
            
            return View(cartItem);
        }


        public IActionResult WishListIndex()
        {
            var id = Convert.ToString(HttpContext.Request.Cookies["CartId"]);
            IEnumerable<CartViewModel> cartItem = _cartItemRepository.Get(c => c.Cart.Cart_Id == id && c.WantToBuy == false, includeProperties: "Price,Book,Cart")
                .Select(c => new CartViewModel
                {
                    BookId = c.Book.Book_Id,
                    Url = c.Book.FrontUrl,
                    Prices = c.Price.ItemPrice,
                    BookName = c.Book.Name,
                    CartItem_Id = c.CartItem_Id,
                    Quantity = c.Price.Quantity,
                    PriceId = c.Price.Price_Id,
                });

            return View(cartItem);

        }
        [HttpPost]
        public IActionResult MoveToCart(string id)
        {


            var cartItem = _cartItemRepository.Get(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            cartItem.WantToBuy = true;
            _cartItemRepository.Update(cartItem);
            _cartItemRepository.Save();
            return RedirectToAction("WishListIndex");
        }

        [HttpPost]
        public IActionResult AllMoveToCart()
        {
            var id = Convert.ToString(HttpContext.Request.Cookies["CartId"]);

            IEnumerable<CartItem> cartItem = _cartItemRepository.Get(c => c.Cart.Cart_Id == id && c.WantToBuy == false);

            foreach (CartItem item in cartItem)
            {

                item.WantToBuy = true;
                _cartItemRepository.Update(item);
                _cartItemRepository.Save();

            }


            return RedirectToAction("WishListIndex");
        }

        public async Task<IActionResult> CheckOut(string[] fruits,string[] IDs,string[] quantity,string[] bookF,string[]priceF)
        {
            //set the origin value
            var orderStatue = _orderStatusRepository.Get(s => s.Status == Constants.OrderStatusPending).FirstOrDefault();
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var customer = _customerRepository.Get(userId);
            decimal subTotal = 0;
            DateTime date = DateTime.Now.ToUniversalTime().AddDays(7);
            var deliveryDate = date.ToString("dd/MM/yyyy");
            //calculate the total price and put all book in a list
            for (int i = 0; i < IDs.Length; i++)
            {
                subTotal += Convert.ToDecimal(fruits[i]) * Convert.ToInt32(quantity[i]);
            }

            //creat a new order
            var recentOrder = new Order() { OrderStatus = orderStatue, Subtotal = subTotal, Tax = subTotal * (decimal)0.1, Customer = customer, DeliveryDate= deliveryDate };
            _orderRepository.Add(recentOrder);
            _orderRepository.Save();
            var orderId = recentOrder.Order_Id;
            if (!HttpContext.Request.Cookies.ContainsKey("OrderId"))
            {
                CookieOptions options = new CookieOptions();

                HttpContext.Response.Cookies.Append("OrderId", Convert.ToString(orderId));


            }
            else
            {
                HttpContext.Response.Cookies.Delete("OrderId");
                HttpContext.Response.Cookies.Append("OrderId", Convert.ToString(orderId));
            }
            //add item to these order
            for (int i = 0; i < bookF.Length; i++)
            {
                
                var orderDetailBook = _bookRepository.Get(Convert.ToInt64(bookF[i]));
                var orderDetailPrice =_priceRepository.Get(Convert.ToInt64(priceF[i]));
                var newOrderDetail = new OrderDetail() { };
                var OrderDetail = new OrderDetail() { Book = orderDetailBook, Price = orderDetailPrice, OrderDetailPrice =Convert.ToDecimal(fruits[i]), Quantity = Convert.ToInt32(quantity[i]), Order = recentOrder, IsRemoved = false };
                orderDetailPrice.Quantity = orderDetailPrice.Quantity - Convert.ToInt32(quantity[i]);
                _priceRepository.Update(orderDetailPrice);
                _orderDetailRepository.Add(OrderDetail);
                _priceRepository.Save();
                _orderDetailRepository.Save();
            }
            //remove from cart
            for (int i = 0; i < IDs.Length; i++)
            {
                var cartItemD = _cartItemRepository.Get(Convert.ToString (IDs[i]));
                _cartItemRepository.Remove(cartItemD);
            }

            _cartItemRepository.Save();
            return RedirectToAction(nameof(ConfirmCheckout),new { OrderId=orderId});
        }

        public async Task<IActionResult> ConfirmCheckout(long OrderId)
        {
            var user = await _userManager.GetUserAsync(User);
            var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var customer = _customerRepository.Get(id);
            var address = _addressRepository.Get(c => c.Customer == customer);

            var order = _orderRepository.Get(OrderId);
            var orderDeteail = _orderDetailRepository.Get(m=> m.Order == order, includeProperties:"Book")
                               .Select(m=> new OrderDetailViewModel()
                               { Bookname=m.Book.Name,
                               Url=m.Book.FrontUrl,
                               Price=m.OrderDetailPrice,
                               Quantity=m.Quantity
                               });
            ViewData["order"] = orderDeteail.ToList();
            ViewData["orderId"] = OrderId;
            return View(address.ToList());
        }

        public IActionResult Congratulation(long OrderIdC)
        {
            var order = _orderRepository.Get(OrderIdC);
            var OrderItem = _orderDetailRepository.Get(c => c.Order == order, includeProperties:"Book")
                           .Select(c => new OrderDetailViewModel()
                           {
                               Bookname = c.Book.Name,
                               Url = c.Book.FrontUrl,
                               Price = c.OrderDetailPrice,
                               Quantity = c.Quantity
                           });
            ViewData["order"] = OrderItem.ToList();
            return View();
        }

        public IActionResult ConfirmOrderAddress(string addressID, long OrderId)
        {
            var address = _addressRepository.Get(Convert.ToInt64(addressID));
            var order = _orderRepository.Get(OrderId);
            order.Address = address;
            _orderRepository.Update(order);
            _orderRepository.Save();
            return RedirectToAction(nameof(Congratulation), new { OrderIdC = OrderId });
        }

        public IActionResult AddAddressAtChechout()
        {
            return View();
        }

        public async Task<IActionResult> AddAddressAtChechoutsave([Bind("Address_Id,AddressLine1,AddressLine2,City,State,Country,ZipCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                var customer = _customerRepository.Get(id);
                address.Customer = customer;
                _addressRepository.Add(address);
                _addressRepository.Save();
                var orderId = Convert.ToInt64(HttpContext.Request.Cookies["OrderId"]);
                return RedirectToAction(nameof(ConfirmCheckout), new { OrderId = orderId });
            }
            return View(address);
        }

        public IActionResult EditAddress(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = _addressRepository.Get(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(long id, [Bind("Address_Id,AddressLine1,AddressLine2,City,State,Country,ZipCode")] Address address)
        {
            if (id != address.Address_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                _addressRepository.Update(address);
                _addressRepository.Save();
                var orderId = Convert.ToInt64(HttpContext.Request.Cookies["OrderId"]);
                return RedirectToAction(nameof(ConfirmCheckout), new { OrderId = orderId });
                
            }
            return View(address);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(m => m.CartItem_Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var cartItem = _cartItemRepository.Get(c => c.CartItem_Id == id, includeProperties:"Cart,Cart.Customer").FirstOrDefault();
                


            if (cartItem.Cart.Customer.Customer_Id!=userId)
            {
                return RedirectToAction(nameof(Error));
            }

            var trueDelete = _cartItemRepository.Get(id);
            _cartItemRepository.Remove(trueDelete);
             _cartItemRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View();
        }
        
        private bool CartItemExists(string id)
        {
            return _context.CartItem.Any(e => e.CartItem_Id == id);
        }
    }
}
