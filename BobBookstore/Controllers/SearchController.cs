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
using PagedList;

namespace BobBookstore.Controllers
{
    public class SearchController : Controller
    {
        private readonly UsedBooksContext _context;

        public SearchController(UsedBooksContext context)
        {
            _context = context;
        }

        // Load Search results page
        // SortBy - value to sort results by
        // searchString - user's search query
        // page - page for results
        public async Task<IActionResult> Index(string SortBy, string searchString, int? page)
        {
            if (!String.IsNullOrEmpty(SortBy))
            {
                ViewBag.CurrentSort = SortBy;
            }
            
            if (String.IsNullOrEmpty(searchString))
            {
                //searchString = ViewBag.currentFilter;
                var prices = from p in _context.Price
                             where p.Quantity > 0 &&
                             p.Active
                             select p;
                var books = from b in _context.Book

                            select new BookViewModel
                            {
                                BookId = b.Book_Id,
                                BookName = b.Name,
                                ISBN = b.ISBN,
                                Author = b.Author,
                                GenreName = b.Genre.Name,
                                TypeName = b.Type.TypeName,
                                PublisherName = b.Publisher.Name,
                                Url = b.Back_Url,
                                Prices = prices.Where(p => p.Book.Book_Id == b.Book_Id).ToList(),
                                MinPrice = prices.Where(p => p.Book.Book_Id == b.Book_Id).FirstOrDefault().ItemPrice
                            };

                int pageSize = 10;
                int currentPage = (page ?? 1);

                return View(new PaginationModel
                {
                    Count = books.Count(),
                    Data = await books.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(),
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    CurrentFilter = searchString
                });
            }
            else
            {
                ViewBag.currentFilter = searchString;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.currentFilter = searchString;
                var prices = from p in _context.Price
                             where p.Quantity > 0 &&
                             p.Active && (
                            p.Book.Name.Contains(searchString) ||
                            p.Book.Genre.Name.Contains(searchString) ||
                            p.Book.Type.TypeName.Contains(searchString) ||
                            p.Book.ISBN.ToString().Contains(searchString)) ||
                            p.Book.Publisher.Name.Contains(searchString)
                             select p;

                prices = prices.OrderBy(p => p.ItemPrice);

                
                var books = from b in _context.Book
                            where b.Name.Contains(searchString) ||
                            b.Genre.Name.Contains(searchString) ||
                            b.Type.TypeName.Contains(searchString) ||
                            b.ISBN.ToString().Contains(searchString)||
                            b.Publisher.Name.Contains(searchString)
                            select new BookViewModel
                            {
                                BookId = b.Book_Id,
                                BookName = b.Name,
                                ISBN = b.ISBN,
                                Author = b.Author,
                                GenreName = b.Genre.Name,
                                TypeName = b.Type.TypeName,
                                PublisherName = b.Publisher.Name,
                                Url = b.Back_Url,
                                Prices = prices.Where(p => p.Book.Book_Id == b.Book_Id).ToList(),
                                MinPrice = prices.Where(p => p.Book.Book_Id == b.Book_Id).FirstOrDefault().ItemPrice
                            };

                // sort query
                switch (ViewBag.CurrentSort)
                {
                    case "Name":
                        books = books.OrderByDescending(b => b.BookName);
                        break;
                    case "Genre":
                        books = books.OrderBy(b => b.GenreName);
                        break;
                    case "Type":
                        books = books.OrderBy(b => b.TypeName);
                        break;
                    case "PriceAsc":
                        books = books.OrderBy(b => b.MinPrice);
                        break;
                    case "PriceDesc":
                        books = books.OrderByDescending(b => b.MinPrice);
                        break;
                    default:
                        books = books.OrderBy(b => b.BookName);
                        break;
                }

                int pageSize = 10;
                int currentPage = (page ?? 1);
                
                return View(new PaginationModel 
                    {
                    Count = books.Count(),
                    Data = await books.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(),
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    CurrentFilter = searchString});

            }

            return View();
        }

        // Load book details page
        public async Task<IActionResult> DetailAsync(long id, string sortBy)
        { 
            if (!String.IsNullOrEmpty(sortBy))
            {
                ViewBag.CurrentSort = sortBy;
            }
            ViewBag.id = id;


            var prices = (from p in _context.Price where p.Book.Book_Id == id &&
                          p.Active && p.Quantity > 0
                         join c in _context.Condition
                         on p.Condition.Condition_Id equals c.Condition_Id
                         select new Price
                         {
                             Price_Id = p.Price_Id,
                             Condition = c,
                             ItemPrice = p.ItemPrice,
                             Quantity = p.Quantity
                         });

            switch (sortBy)
            {
                case "priceAsc":
                    prices = prices.OrderBy(p => p.ItemPrice);
                    break;
                case "priceDesc":
                    prices = prices.OrderByDescending(p => p.ItemPrice);
                    break;
                case "conditionAsc":
                    prices = prices.OrderBy(p => p.Condition);
                    break;
                case "conditionDesc":
                    prices = prices.OrderByDescending(p => p.Condition);
                    break;
                default:
                    prices = prices.OrderBy(p => p.ItemPrice);
                    break;
            }
            var pricesLst = await prices.ToListAsync();

            var book = from m in _context.Book
                       where m.Book_Id == id
                       select new BookViewModel()
                       {
                           BookName = m.Name,
                           Author = m.Author,
                           PublisherName = m.Publisher.Name,
                           ISBN = m.ISBN,
                           GenreName = m.Genre.Name,
                           TypeName = m.Type.TypeName,
                           Url = m.Back_Url,
                           Prices = pricesLst,
                           BookId = m.Book_Id,
                           Summary = m.Summary
                       };

            return View(await book.FirstOrDefaultAsync());
        }

        public async Task<IActionResult> AddtoCartitem(long bookid, long priceid)
        {
            var book = _context.Book.Find(bookid);
            var price = _context.Price.Find(priceid);
            var cartId = HttpContext.Request.Cookies["CartId"];
            var cart = _context.Cart.Find(Convert.ToString (cartId));
            Guid gu_id = Guid.NewGuid();
            var cartItem = new CartItem() { Book = book, Price = price, Cart = cart,WantToBuy=true, CartItem_Id = gu_id.ToString() };

            _context.Add(cartItem);
            _context.SaveChanges();
            var stringBookid = Convert.ToString(bookid);
            return RedirectToAction("Detail","Search",new { @id = bookid });
        }
        public async Task<IActionResult> AddtoWishList(long bookid, long priceid)
        {
            var book = _context.Book.Find(bookid);
            var price = _context.Price.Find(priceid);
            var cartId = HttpContext.Request.Cookies["CartId"];
            var cart = _context.Cart.Find(Convert.ToString(cartId));
            Guid gu_id = Guid.NewGuid();
            var cartItem = new CartItem() { Book = book, Price = price, Cart = cart,CartItem_Id=gu_id.ToString()};

            _context.Add(cartItem);
            _context.SaveChanges();
            var stringBookid = Convert.ToString(bookid);
            return RedirectToAction("Detail", "Search", new { @id = bookid });
        }
    }
}
