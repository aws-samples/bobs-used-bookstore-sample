using Bookstore.Customer.ViewModel.Checkout;
using Bookstore.Domain.Carts;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Customer.Mappers
{
    public static class ShoppingCartItemToShoppingCartCheckoutItemViewModelMapper
    {
        public static List<CheckoutItemViewModel> ToShoppingCartCheckoutItemViewModels(this IEnumerable<ShoppingCartItem> shoppingCartItems)
        {
            return shoppingCartItems.Select(x => new CheckoutItemViewModel
            {
                BookName = x.Book.Name,
                ImageUrl = x.Book.FrontImageUrl,
                Price= x.Book.Price
            }).ToList();
        }
    }
}
