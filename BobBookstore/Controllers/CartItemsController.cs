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

namespace BobBookstore.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly UsedBooksContext _context;

        public CartItemsController(UsedBooksContext context)
        {
            _context = context;
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(HttpContext.Request.Cookies["CartId"]);
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
                           quantity=c.Price.Quantity

                       };
            
            return View(await cartItem.ToListAsync());
            //return View(Tuple.Create(item1,book));
            
        }
        public async Task<IActionResult> WishListIndex()
        {
            var id = Convert.ToInt32(HttpContext.Request.Cookies["CartId"]);
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
                               quantity = c.Price.Quantity

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
        

        public async Task<IActionResult> AddtoCartitem(long bookid,long priceid)
        {
            var book = _context.Book.Find(bookid);
            var price = _context.Price.Find(priceid);
            var cartId = HttpContext.Request.Cookies["CartId"];
            var cart = _context.Cart.Find(Convert.ToInt32(cartId));

            var cartItem = new CartItem() { Book = book, Price = price, Cart = cart,WantToBuy=true };

            _context.Add(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddtoWishlist(long bookid, long priceid)
        {
            var book = _context.Book.Find(bookid);
            var price = _context.Price.Find(priceid);
            var cartId = HttpContext.Request.Cookies["CartId"];
            var cart = _context.Cart.Find(Convert.ToInt32(cartId));

            var cartItem = new CartItem() { Book = book, Price = price, Cart = cart, WantToBuy = true };

            _context.Add(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: CartItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartItem_Id")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartItem_Id")] CartItem cartItem)
        {
            if (id != cartItem.CartItem_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.CartItem_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.CartItem.FindAsync(id);
            _context.CartItem.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItem.Any(e => e.CartItem_Id == id);
        }
    }
}
