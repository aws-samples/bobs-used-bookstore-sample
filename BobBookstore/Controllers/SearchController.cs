using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobBookstore.Data;
using BobBookstore.Models.Book;
using BobBookstore.Models.Carts;
using BobBookstore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BobBookstore.Controllers
{
    public class SearchController : Controller
    {
        private readonly UsedBooksContext _context;

        public SearchController(UsedBooksContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync(string searchString)
        {

            if (!String.IsNullOrEmpty(searchString))
            {
                var books = from b in _context.Book
                            where b.Name.Contains(searchString) ||
                            b.Genre.Name.Contains(searchString) ||
                            b.Type.TypeName.Contains(searchString) ||
                            b.ISBN.ToString().Contains(searchString)
                            select new BookViewModel()
                            {
                                BookId = b.Book_Id,
                                BookName = b.Name,
                                ISBN = b.ISBN,
                                GenreName = b.Genre.Name,
                                TypeName = b.Type.TypeName,
                                Url = b.Back_Url
                            };
                return View(await books.ToListAsync());

            }

            return View();
        }

        public async Task<IActionResult> DetailAsync(long id)
        {
            var prices = await (from p in _context.Price
                         where p.Book.Book_Id == id
                         select p).ToListAsync();

            var book = from m in _context.Book
                       where m.Book_Id == id
                       select new BookViewModel()
                       {
                           BookName = m.Name,
                           ISBN = m.ISBN,
                           GenreName = m.Genre.Name,
                           TypeName = m.Type.TypeName,
                           Url = m.Back_Url,
                           Prices = prices
                       };

            return View(await book.FirstOrDefaultAsync());
        }

        public async Task<IActionResult> AddtoCartitem(long bookid, long priceid)
        {
            var book = _context.Book.Find(bookid);
            var price = _context.Price.Find(priceid);
            var cartId = HttpContext.Request.Cookies["CartId"];
            var cart = _context.Cart.Find(Convert.ToInt32(cartId));

            var cartItem = new CartItem() { Book = book, Price = price, Cart = cart };

            _context.Add(cartItem);
            _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
