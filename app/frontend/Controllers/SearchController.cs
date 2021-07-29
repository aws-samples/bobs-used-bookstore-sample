using System;
using System.Linq;
using System.Threading.Tasks;
using BobsBookstore.Models.Books;
using BobsBookstore.Models.Carts;
using BobsBookstore.Models.ViewModels;
using BobsBookstore.DataAccess.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;

namespace BobBookstore.Controllers
{
    public class SearchController : Controller
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<Price> _priceRepository;
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<CartItem> _cartItemRepository;
        private readonly IGenericRepository<Condition> _conditionRepository;
        private readonly IBookSearch _bookSearch;
        private readonly IPriceSearch _priceSearch;

        public SearchController(IBookSearch bookSearch, IPriceSearch priceSearch, IGenericRepository<Condition> conditionRepository, IGenericRepository<CartItem> cartItemRepository, IGenericRepository<Cart> cartRepository, IGenericRepository<Price> priceRepository, IGenericRepository<Book> bookRepository)
        {
            _priceRepository = priceRepository;
            _bookRepository = bookRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _conditionRepository = conditionRepository;
            _priceSearch = priceSearch;
            _bookSearch = bookSearch;
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
                var prices = _priceRepository.Get(p => p.Quantity > 0 &&
                             p.Active);

                var books = _bookRepository.Get(includeProperties: "Genre,Type,Publisher")
                    .Select(b => new BookViewModel
                    {
                        BookId = b.Book_Id,
                        BookName = b.Name,
                        ISBN = b.ISBN,
                        Author = b.Author,
                        GenreName = b.Genre.Name,
                        TypeName = b.Type.TypeName,
                        PublisherName = b.Publisher.Name,
                        Url = b.BackUrl,
                        Prices = prices.Where(p => p.Book.Book_Id == b.Book_Id).ToList(),
                        MinPrice = prices.Where(p => p.Book.Book_Id == b.Book_Id).FirstOrDefault().ItemPrice
                    });

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
                var pricesQuery = _priceSearch.GetPricebySearch(searchString).OrderBy(d => d.ItemPrice);

                var booksQuery = _bookSearch.GetBooksbySearch(searchString)
                            .Select(b => new BookViewModel
                            {
                                BookId = b.Book_Id,
                                BookName = b.Name,
                                ISBN = b.ISBN,
                                Author = b.Author,
                                GenreName = b.Genre.Name,
                                TypeName = b.Type.TypeName,
                                PublisherName = b.Publisher.Name,
                                Url = b.BackUrl,
                                Prices = pricesQuery.Where(p => p.Book.Book_Id == b.Book_Id).ToList(),
                                MinPrice = pricesQuery.Where(p => p.Book.Book_Id == b.Book_Id).FirstOrDefault().ItemPrice
                            });

                // sort query
                switch (ViewBag.CurrentSort)
                {
                    case "Name":
                        booksQuery = booksQuery.OrderByDescending(b => b.BookName);
                        break;
                    case "Genre":
                        booksQuery = booksQuery.OrderBy(b => b.GenreName);
                        break;
                    case "Type":
                        booksQuery = booksQuery.OrderBy(b => b.TypeName);
                        break;
                    case "PriceAsc":
                        booksQuery = booksQuery.OrderBy(b => b.MinPrice);
                        break;
                    case "PriceDesc":
                        booksQuery = booksQuery.OrderByDescending(b => b.MinPrice);
                        break;
                    default:
                        booksQuery = booksQuery.OrderBy(b => b.BookName);
                        break;
                }

                int pageSize = 10;
                int currentPage = (page ?? 1);

                return View(new PaginationModel
                {
                    Count = booksQuery.Count(),
                    Data = await booksQuery.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(),
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    CurrentFilter = searchString
                });

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

            var prices = _priceRepository.Get(p => p.Book.Book_Id == id && p.Active && p.Quantity > 0, includeProperties: "Condition,Book");

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

            var book = _bookRepository.Get(m => m.Book_Id == id, includeProperties: "Publisher,Genre,Type")
                       .Select(m => new BookViewModel()
                       {
                           BookName = m.Name,
                           Author = m.Author,
                           PublisherName = m.Publisher.Name,
                           ISBN = m.ISBN,
                           GenreName = m.Genre.Name,
                           TypeName = m.Type.TypeName,
                           Url = m.BackUrl,
                           Prices = pricesLst,
                           BookId = m.Book_Id,
                           Summary = m.Summary
                       });

            return View(book.FirstOrDefault());
        }

        public IActionResult AddtoCartitem(long bookid, long priceid)
        {
            var book = _bookRepository.Get(bookid);
            var price = _priceRepository.Get(priceid);
            var cartId = HttpContext.Request.Cookies["CartId"];
            var cart = _cartRepository.Get(Convert.ToString(cartId));
            Guid gu_id = Guid.NewGuid();
            var cartItem = new CartItem() { Book = book, Price = price, Cart = cart, WantToBuy = true, CartItem_Id = gu_id.ToString() };

            _cartItemRepository.Add(cartItem);
            _cartItemRepository.Save();
            var stringBookid = Convert.ToString(bookid);
            return RedirectToAction("Detail", "Search", new { @id = bookid });
        }

        public IActionResult AddtoWishList(long bookid, long priceid)
        {
            var book = _bookRepository.Get(bookid);
            var price = _priceRepository.Get(priceid);
            var cartId = HttpContext.Request.Cookies["CartId"];
            var cart = _cartRepository.Get(Convert.ToString(cartId));
            Guid gu_id = Guid.NewGuid();
            var cartItem = new CartItem() { Book = book, Price = price, Cart = cart, CartItem_Id = gu_id.ToString() };

            _cartItemRepository.Add(cartItem);
            _cartItemRepository.Save();
            return RedirectToAction("Detail", "Search", new { @id = bookid });
        }
    }
}
