using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.Domain.Orders;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Books;
using Bookstore.Domain.Customers;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface;
using Bookstore.Customer;
using Bookstore.Customer.ViewModel;
using Bookstore.Services;
using Bookstore.Customer.ViewModel.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Customer.Mappers;

namespace CustomerSite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IGenericRepository<Address> _addressRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<ShoppingCartItem> _cartItemRepository;
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly ICustomerService customerService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IGenericRepository<OrderItem> _orderDetailRepository;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Price> _priceRepository;

        public ShoppingCartController(IGenericRepository<Address> addressRepository,
                                   IGenericRepository<OrderItem> orderDetailRepository,
                                   IGenericRepository<Order> orderRepository,
                                   IGenericRepository<Price> priceRepository,
                                   IGenericRepository<Book> bookRepository,
                                   IGenericRepository<Customer> customerRepository,
                                   ICustomerService customerService,
                                   IShoppingCartService shoppingCartService,
                                   IGenericRepository<ShoppingCartItem> cartItemRepository,
                                   ApplicationDbContext context)
        {
            _context = context;
            _cartItemRepository = cartItemRepository;
            _customerRepository = customerRepository;
            this.customerService = customerService;
            this.shoppingCartService = shoppingCartService;
            _bookRepository = bookRepository;
            _priceRepository = priceRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _addressRepository = addressRepository;
        }

        public IActionResult Index()
        {
            var shoppingCartItems = shoppingCartService.GetShoppingCartItems(HttpContext.GetShoppingCartId());
            var viewModels = shoppingCartItems.Select(c => new CartViewModel
            {
                BookId = c.Book.Id,
                Url = c.Book.FrontImageUrl,
                Prices = c.Book.Price,
                BookName = c.Book.Name,
                CartItem_Id = c.Id,
                Quantity = c.Quantity
            });

            return View(viewModels);
        }

        public IActionResult WishListIndex()
        {
            var cartItem
                = _cartItemRepository
                    .Get(c => c.ShoppingCart.CorrelationId == HttpContext.GetShoppingCartId() && c.WantToBuy == false,
                            includeProperties: "Book,ShoppingCart")
                    .Select(c => new CartViewModel
                    {
                        BookId = c.Book.Id,
                        Url = c.Book.FrontImageUrl,
                        //Prices = c.Price.ItemPrice,
                        BookName = c.Book.Name,
                        CartItem_Id = c.Id,
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
            var id = Convert.ToInt32(HttpContext.Request.Cookies["CartId"]);

            var cartItem = _cartItemRepository.Get(c => c.ShoppingCart.Id == id && c.WantToBuy == false);

            foreach (var item in cartItem)
            {
                item.WantToBuy = true;
                _cartItemRepository.Update(item);
                _cartItemRepository.Save();
            }

            return RedirectToAction("WishListIndex");
        }
        
        public IActionResult Delete(int id)
        {
            return View(new ShoppingCartItemDeleteViewModel {  Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ShoppingCartItemDeleteViewModel model)
        {
            await shoppingCartService.DeleteShoppingCartItemAsync(HttpContext.GetShoppingCartId(), model.Id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
