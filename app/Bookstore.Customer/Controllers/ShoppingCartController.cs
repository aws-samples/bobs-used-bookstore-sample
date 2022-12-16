using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Customer;
using Bookstore.Customer.ViewModel;
using Bookstore.Services;
using Bookstore.Customer.ViewModel.ShoppingCart;

namespace CustomerSite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(ICustomerService customerService, IShoppingCartService shoppingCartService)
        {
            this.customerService = customerService;
            this.shoppingCartService = shoppingCartService;
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
