using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BobBookstore.Data;
using BobBookstore.Models.Carts;
using BobBookstore.Models.Book;
using BobBookstore.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using BobBookstore.Models.Order;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Amazon.AspNetCore.Identity.Cognito;
using BobBookstore.Models.Customer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Components.Web;

namespace BobBookstore.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly UsedBooksContext _context;
        private readonly SignInManager<CognitoUser> _SignInManager;
        private readonly UserManager<CognitoUser> _userManager;
        public CartItemsController(UsedBooksContext context, SignInManager<CognitoUser> SignInManager, UserManager<CognitoUser> userManager)
        {
            _context = context;
            _SignInManager = SignInManager;
            _userManager = userManager;
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            
            var id = Convert.ToString (HttpContext.Request.Cookies["CartId"]);
            var cart = _context.Cart.Find(id);
            var cartItem = from c in _context.CartItem
                           where c.Cart==cart&&c.WantToBuy==true
                       select new CartViewModel()
                       {
                           BookId=c.Book.Book_Id,
                           Url=c.Book.Back_Url,
                           Prices=c.Price.ItemPrice,
                           BookName=c.Book.Name,
                           CartItem_Id=c.CartItem_Id,
                           quantity=c.Price.Quantity,
                           PriceId=c.Price.Price_Id,

                       };
            
            return View(await cartItem.ToListAsync());
            
            
        }
        public async Task<IActionResult> WishListIndex()
        {
            var id = Convert.ToString(HttpContext.Request.Cookies["CartId"]);
            var cart = _context.Cart.Find(id);
            var cartItem = from c in _context.CartItem
                           where c.Cart == cart && c.WantToBuy==false
                           select new CartViewModel()
                           {
                               BookId = c.Book.Book_Id,
                               Url = c.Book.Back_Url,
                               Prices = c.Price.ItemPrice,
                               BookName = c.Book.Name,
                               CartItem_Id = c.CartItem_Id,
                               quantity = c.Price.Quantity,
                               PriceId = c.Price.Price_Id,

                           };

            return View(await cartItem.ToListAsync());
            //return View(Tuple.Create(item1,book));

        }
        public async Task<IActionResult> MoveToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            cartItem.WantToBuy = true;
            _context.Update(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("WishListIndex");
        }
        [HttpPost]
        public async Task<IActionResult> AllMoveToCart(string[] fruits)
        {
            string a = "1,";
            foreach (var ids in fruits)
            {
                var id = Convert.ToInt32(ids);
                if (id == null)
                {
                    return NotFound();
                }

                var cartItem = await _context.CartItem.FindAsync(id);
                if (cartItem == null)
                {
                    return NotFound();
                }
                cartItem.WantToBuy = true;
                _context.Update(cartItem);
            }



            await _context.SaveChangesAsync();

            return RedirectToAction("WishListIndex");
        }
        
        //add book to cart
        //public async Task<IActionResult> AddtoCartitem(long bookid,long priceid)
        //{
        //    var book = _context.Book.Find(bookid);
        //    var price = _context.Price.Find(priceid);
        //    var cartId = HttpContext.Request.Cookies["CartId"];
        //    var cart = _context.Cart.Find(Convert.ToInt32(cartId));

        //    var cartItem = new CartItem() { Book = book, Price = price, Cart = cart,WantToBuy=true };

        //    _context.Add(cartItem);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        //public async Task<IActionResult> AddtoWishlist(long bookid, long priceid)
        //{
        //    var book = _context.Book.Find(bookid);
        //    var price = _context.Price.Find(priceid);
        //    var cartId = HttpContext.Request.Cookies["CartId"];
        //    var cart = _context.Cart.Find(Convert.ToInt32(cartId));

        //    var cartItem = new CartItem() { Book = book, Price = price, Cart = cart, WantToBuy = true };

        //    _context.Add(cartItem);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        public async Task<IActionResult> CheckOut(string[] fruits,string[] IDs,string[] quantity,string[] bookF,string[]priceF)
        {

            //set the origin value
            long statueId = 1;
            var orderStatue = _context.OrderStatus.Find(statueId);
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var customer = _context.Customer.Find(userId);
            double subTotal = 0.0;
            var itemIdList = new List<CartItem>();
            //calculate the total price and put all book in a list
            for (int i = 0; i < IDs.Length; i++)
            {
                var cartItem = from c in _context.CartItem
                               where c.CartItem_Id == Convert.ToString (IDs[i])
                               select c;
                              
                var item = new CartItem();
                foreach (var ii in cartItem)
                {
                    item = ii;
                }
                
                itemIdList.Add(item);
                subTotal += Convert.ToDouble( fruits[i]) * Convert.ToInt32(quantity[i]);
            }
            //creat a new order
            var recentOrder = new Order() { OrderStatus = orderStatue, Subtotal = subTotal, Tax = subTotal * 0.1, Customer = customer };
            _context.Add(recentOrder);
            _context.SaveChanges();
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
                
                var orderDetailBook = _context.Book.Find(Convert.ToInt64(bookF[i]));
                //var orderDetailPrice = itemIdList[i].Price;
                var orderDetailPrice = _context.Price.Find(Convert.ToInt64(priceF[i]));
                var newOrderDetail = new OrderDetail() { };
                var OrderDetail = new OrderDetail() { Book = orderDetailBook, Price = orderDetailPrice, price =Convert.ToDouble(fruits[i]), quantity = Convert.ToInt32(quantity[i]), Order = recentOrder, IsRemoved = false };
                orderDetailPrice.Quantity = orderDetailPrice.Quantity - Convert.ToInt32(quantity[i]);
                _context.Update(orderDetailPrice);
                _context.Add(OrderDetail);
                _context.SaveChanges();
            }
            //remove from cart
            for (int i = 0; i < IDs.Length; i++)
            {
                var cartItemD = _context.CartItem.Find(Convert.ToString (IDs[i]));
                _context.CartItem.Remove(cartItemD);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ConfirmCheckout),new { OrderId=orderId});
            //return View();
        }
        public async Task<IActionResult> ConfirmCheckout(long OrderId)
        {
            var user = await _userManager.GetUserAsync(User);
            var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var customer = _context.Customer.Find(id);
            var address = from c in _context.Address
                          where c.Customer == customer
                          select c;
            

            var order = _context.Order.Find(OrderId);
            var orderDeteail = from m in _context.OrderDetail
                               where m.Order == order
                               select new OrderDetailViewModel()
                               { Bookname=m.Book.Name,
                               Url=m.Book.Back_Url,
                               price=m.price,
                               quantity=m.quantity
                               };
            ViewData["order"] = orderDeteail.ToList();
            ViewData["orderId"] = OrderId;
            return View(address.ToList());
        }
        public async Task<IActionResult> Congratulation(long OrderIdC)
        {
            var order = _context.Order.Find(OrderIdC);
            var OrderItem = from c in _context.OrderDetail
                           where c.Order == order
                           select new OrderDetailViewModel()
                           {
                               Bookname = c.Book.Name,
                               Url = c.Book.Back_Url,
                               price = c.price,
                               quantity = c.quantity
                           };
            ViewData["order"] = OrderItem.ToList();
            return View();
        }
        public async Task<IActionResult> ConfirmOrderAddress(string addressID,long OrderId)
        {
            var address = _context.Address.Find(Convert.ToInt64(addressID));
            var order = _context.Order.Find(OrderId);
            order.Address = address;
            _context.Update(order);
            _context.SaveChanges();
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
                var customer = _context.Customer.Find(id);
                address.Customer = customer;
                _context.Add(address);
                await _context.SaveChangesAsync();
                var orderId = Convert.ToInt64(HttpContext.Request.Cookies["OrderId"]);
                return RedirectToAction(nameof(ConfirmCheckout), new { OrderId = orderId });
            }
            return View(address);
        }


        public async Task<IActionResult> EditAddress(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FindAsync(id);
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
                
                _context.Update(address);
                await _context.SaveChangesAsync();
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
            var cartItem = from c in _context.CartItem
                           where c.CartItem_Id == id
                           select new CartDelete
                           {customerId=c.Cart.Customer.Customer_Id
                           };

            //var list = await cartItem.ToListAsync();
            
            var ccc = new CartDelete();
            foreach (var item in cartItem)
            {
                ccc = item;
            }
            if (ccc.customerId!=userId)
            {
                

                return RedirectToAction(nameof(Error));
            }
            var trueDelete = _context.CartItem.Find(id);
            _context.CartItem.Remove(trueDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Error()
        {
            return View();
        }
        private bool CartItemExists(string id)
        {
            return _context.CartItem.Any(e => e.CartItem_Id == id);
        }
    }
}
