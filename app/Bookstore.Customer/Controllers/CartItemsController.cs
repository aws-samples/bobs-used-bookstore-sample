using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using BobsCustomerSite.Models.ViewModels;
using CustomerSite.Models.ViewModels;
using Bookstore.Domain.Orders;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Books;
using Bookstore.Domain.Customers;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface;

namespace CustomerSite.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly IGenericRepository<Address> _addressRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<CartItem> _cartItemRepository;
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderStatus> _orderStatusRepository;
        private readonly IGenericRepository<Price> _priceRepository;
        private readonly UserManager<CognitoUser> _userManager;

        public CartItemsController(IGenericRepository<Address> addressRepository,
                                   IGenericRepository<OrderDetail> orderDetailRepository,
                                   IGenericRepository<Order> orderRepository,
                                   IGenericRepository<Price> priceRepository,
                                   IGenericRepository<Book> bookRepository,
                                   IGenericRepository<Customer> customerRepository,
                                   IGenericRepository<OrderStatus> orderStatusRepository,
                                   IGenericRepository<CartItem> cartItemRepository,
                                   ApplicationDbContext context,
                                   UserManager<CognitoUser> userManager)
        {
            _context = context;
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

        public IActionResult Index()
        {
            var id = Convert.ToString(HttpContext.Request.Cookies["CartId"]);

            var cartItem
                = _cartItemRepository
                    .Get(c => c.Cart.Cart_Id == id && c.WantToBuy == true,
                            includeProperties: "Price,Book,Cart")
                    .Select(c => new CartViewModel
                    {
                        BookId = c.Book.Id,
                        Url = c.Book.FrontImageUrl,
                        //Prices = c.Price.ItemPrice,
                        BookName = c.Book.Name,
                        CartItem_Id = c.CartItem_Id,
                        //Quantity = c.Price.Quantity,
                        //PriceId = c.Price.Price_Id
                    });

            return View(cartItem);
        }

        public IActionResult WishListIndex()
        {
            var id = Convert.ToString(HttpContext.Request.Cookies["CartId"]);
            var cartItem
                = _cartItemRepository
                    .Get(c => c.Cart.Cart_Id == id && c.WantToBuy == false,
                            includeProperties: "Price,Book,Cart")
                    .Select(c => new CartViewModel
                    {
                        BookId = c.Book.Id,
                        Url = c.Book.FrontImageUrl,
                        //Prices = c.Price.ItemPrice,
                        BookName = c.Book.Name,
                        CartItem_Id = c.CartItem_Id,
                        //Quantity = c.Price.Quantity,
                        //PriceId = c.Price.Price_Id
                    });

            return View(cartItem);
        }

        [HttpPost]
        public IActionResult MoveToCart(string id)
        {
            var cartItem = _cartItemRepository.Get(id);
            if (cartItem == null)
                return NotFound();

            cartItem.WantToBuy = true;
            _cartItemRepository.Update(cartItem);
            _cartItemRepository.Save();

            return RedirectToAction("WishListIndex");
        }

        [HttpPost]
        public IActionResult AllMoveToCart()
        {
            var id = Convert.ToString(HttpContext.Request.Cookies["CartId"]);

            var cartItem = _cartItemRepository.Get(c => c.Cart.Cart_Id == id && c.WantToBuy == false);

            foreach (var item in cartItem)
            {
                item.WantToBuy = true;
                _cartItemRepository.Update(item);
                _cartItemRepository.Save();
            }

            return RedirectToAction("WishListIndex");
        }

        public async Task<IActionResult> CheckOut(string[] prices, string[] IDs, string[] quantity, string[] bookF, string[] priceF)
        {
            //set the origin value
            var orderStatue
                = _orderStatusRepository
                    .Get(s => s.Status == Constants.OrderStatusPending)
                    .FirstOrDefault();

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var customer = _customerRepository.Get(userId);

            decimal subTotal = 0;
            var date = DateTime.Now.ToUniversalTime().AddDays(7);
            var deliveryDate = date.ToString("dd/MM/yyyy");

            // calculate the total price and put all books in a list
            for (var i = 0; i < IDs.Length; i++)
            {
                subTotal += Convert.ToDecimal(prices[i]) * Convert.ToInt32(quantity[i]);
            }

            // create a new order
            var recentOrder = new Order
            {
                OrderStatus = orderStatue,
                Subtotal = subTotal,
                Tax = subTotal * (decimal)0.1,
                Customer = customer,
                DeliveryDate = deliveryDate
            };

            _orderRepository.Add(recentOrder);
            _orderRepository.Save();

            var orderId = recentOrder.Id;
            if (!HttpContext.Request.Cookies.ContainsKey("OrderId"))
            {
                HttpContext.Response.Cookies.Append("OrderId", Convert.ToString(orderId));
            }
            else
            {
                HttpContext.Response.Cookies.Delete("OrderId");
                HttpContext.Response.Cookies.Append("OrderId", Convert.ToString(orderId));
            }

            // add item to these order
            for (var i = 0; i < bookF.Length; i++)
            {
                var orderDetailBook = _bookRepository.Get(Convert.ToInt64(bookF[i]));
                var orderDetailPrice = _priceRepository.Get(Convert.ToInt64(priceF[i]));

                var orderDetail = new OrderDetail
                {
                    Book = orderDetailBook,
                    //Price = orderDetailPrice,
                    OrderDetailPrice = Convert.ToDecimal(prices[i]),
                    Quantity = Convert.ToInt32(quantity[i]),
                    Order = recentOrder,
                    IsRemoved = false
                };

                orderDetailPrice.Quantity -= Convert.ToInt32(quantity[i]);
                _priceRepository.Update(orderDetailPrice);
                _orderDetailRepository.Add(orderDetail);
                _priceRepository.Save();
                _orderDetailRepository.Save();
            }

            // remove from cart
            foreach (var t in IDs)
            {
                var cartItemD = _cartItemRepository.Get(t);
                _cartItemRepository.Remove(cartItemD);
            }

            _cartItemRepository.Save();
            return RedirectToAction(nameof(ConfirmCheckout), new { OrderId = orderId });
        }

        public async Task<IActionResult> ConfirmCheckout(long orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var customer = _customerRepository.Get(id);
            var address = _addressRepository.Get(c => c.Customer == customer);

            var order = _orderRepository.Get(orderId);
            var orderDetail = _orderDetailRepository.Get(m => m.Order == order, includeProperties: "Book")
                .Select(m => new OrderDetailViewModel
                {
                    Bookname = m.Book.Name,
                    Url = m.Book.FrontImageUrl,
                    Price = m.OrderDetailPrice,
                    Quantity = m.Quantity
                });

            ViewData["order"] = orderDetail.ToList();
            ViewData["orderId"] = orderId;

            return View(address.ToList());
        }

        public IActionResult OrderPlaced(long orderIdC)
        {
            var order = _orderRepository.Get(orderIdC);
            var orderItem = _orderDetailRepository.Get(c => c.Order == order, includeProperties: "Book")
                .Select(c => new OrderDetailViewModel
                {
                    Bookname = c.Book.Name,
                    Url = c.Book.FrontImageUrl,
                    Price = c.OrderDetailPrice,
                    Quantity = c.Quantity
                });
            
            ViewData["order"] = orderItem.ToList();
            return View();
        }

        public IActionResult ConfirmOrderAddress(string addressId, long orderId)
        {
            var address = _addressRepository.Get(Convert.ToInt64(addressId));
            var order = _orderRepository.Get(orderId);
            order.Address = address;
            _orderRepository.Update(order);
            _orderRepository.Save();

            return RedirectToAction(nameof(OrderPlaced), new { OrderIdC = orderId });
        }

        public IActionResult AddAddressAtCheckout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddressAtCheckout([Bind("Address_Id,AddressLine1,AddressLine2,City,State,Country,ZipCode")] Address address)
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
                return NotFound();

            var address = _addressRepository.Get(id);
            if (address == null)
                return NotFound();

            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAddress(long id, [Bind("Address_Id,AddressLine1,AddressLine2,City,State,Country,ZipCode")] Address address)
        {
            if (id != address.Address_Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _addressRepository.Update(address);
                _addressRepository.Save();
                var orderId = Convert.ToInt64(HttpContext.Request.Cookies["OrderId"]);
                return RedirectToAction(nameof(ConfirmCheckout), new { OrderId = orderId });
            }

            return View(address);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var cartItem = await _context
                .CartItem
                .FirstOrDefaultAsync(m => m.CartItem_Id == id);

            if (cartItem == null)
                return NotFound();

            return View(cartItem);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var cartItem = _cartItemRepository
                .Get(c => c.CartItem_Id == id, includeProperties: "Cart,Cart.Customer")
                .FirstOrDefault();

            if (cartItem?.Cart.Customer.Customer_Id != userId)
                return RedirectToAction(nameof(Error));

            var trueDelete = _cartItemRepository.Get(id);
            _cartItemRepository.Remove(trueDelete);
            _cartItemRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
