using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Bookstore.Domain.Books;
using Bookstore.Domain.Carts;
using Bookstore.Data.Repository.Interface.SearchImplementations;
using Bookstore.Data.Repository.Interface;
using Bookstore.Customer.ViewModel;
using Bookstore.Services;
using Bookstore.Customer;
using Services;
using Bookstore.Customer.Mappers;

namespace BobCustomerSite.Controllers
{

    public class SearchController : Controller
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IInventoryService inventoryService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IBookSearch _bookSearch;
        private readonly IGenericRepository<ShoppingCartItem> _cartItemRepository;
        private readonly IGenericRepository<ShoppingCart> _cartRepository;
        private readonly IGenericRepository<Condition> _conditionRepository;
        private readonly IGenericRepository<Price> _priceRepository;
        private readonly IPriceSearch _priceSearch;

        public IShoppingCartClientManager ShoppingCartClientManager { get; }

        public SearchController(IBookSearch bookSearch,
                                IPriceSearch priceSearch,
                                IGenericRepository<Condition> conditionRepository,
                                IGenericRepository<ShoppingCartItem> cartItemRepository,
                                IGenericRepository<ShoppingCart> cartRepository,
                                IGenericRepository<Price> priceRepository,
                                IGenericRepository<Book> bookRepository,
                                IInventoryService inventoryService,
                                IShoppingCartService shoppingCartService,
                                IShoppingCartClientManager shoppingCartClientManager)
        {
            _priceRepository = priceRepository;
            _bookRepository = bookRepository;
            this.inventoryService = inventoryService;
            this.shoppingCartService = shoppingCartService;
            ShoppingCartClientManager = shoppingCartClientManager;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _conditionRepository = conditionRepository;
            _priceSearch = priceSearch;
            _bookSearch = bookSearch;
        }

        public async Task<IActionResult> Index(string searchString, string sortBy = "Name", int pageIndex = 1, int pageSize = 10)
        {
            var books = inventoryService.GetBooks(searchString, sortBy, pageIndex, pageSize);

            return View(books.ToSearchIndexViewModel());
        }

        public async Task<IActionResult> DetailAsync(long id, string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
                ViewBag.CurrentSort = sortBy;

            ViewBag.id = id;

            //var prices = _priceRepository.Get(p => p.Book.Id == id && p.Active && p.Quantity > 0,
            //    includeProperties: "Condition,Book");

            //switch (sortBy)
            //{
            //    case "priceAsc":
            //        prices = prices.OrderBy(p => p.ItemPrice);
            //        break;
            //    case "priceDesc":
            //        prices = prices.OrderByDescending(p => p.ItemPrice);
            //        break;
            //    case "conditionAsc":
            //        prices = prices.OrderBy(p => p.Condition);
            //        break;
            //    case "conditionDesc":
            //        prices = prices.OrderByDescending(p => p.Condition);
            //        break;
            //    default:
            //        prices = prices.OrderBy(p => p.ItemPrice);
            //        break;
            //}

            //var pricesLst = await prices.ToListAsync();

            var book = _bookRepository.Get(m => m.Id == id, includeProperties: "Publisher,Genre,BookType, Condition")
                .Select(m => new BookViewModel
                {
                    BookName = m.Name,
                    Author = m.Author,
                    PublisherName = m.Publisher.Text,
                    ISBN = m.ISBN,
                    GenreName = m.Genre.Text,
                    TypeName = m.BookType.Text,
                    Url = m.FrontImageUrl,
                    //Prices = pricesLst,
                    MinPrice = m.Price,
                    Quantity = m.Quantity,
                    BookId = m.Id,
                    Summary = m.Summary
                });

            return View(book.FirstOrDefault());
        }

        public async Task<IActionResult> AddItemToShoppingCart(int bookId)
        {
            var shoppingCartClientId = ShoppingCartClientManager.GetShoppingCartId();

            await shoppingCartService.AddAsync(shoppingCartClientId, bookId, 1);

            return RedirectToAction("Index", "Search");
        }

        public IActionResult AddtoWishList(long bookid, long priceid)
        {
            var book = _bookRepository.Get(bookid);
            var price = _priceRepository.Get(priceid);
            var cartId = HttpContext.Request.Cookies["CartId"];
            var cart = _cartRepository.Get(Convert.ToString(cartId));
            var cartItem = new ShoppingCartItem { Book = book, ShoppingCart = cart };

            _cartItemRepository.Add(cartItem);
            _cartItemRepository.Save();
            return RedirectToAction("Detail", "Search", new { id = bookid });
        }
    }
}
