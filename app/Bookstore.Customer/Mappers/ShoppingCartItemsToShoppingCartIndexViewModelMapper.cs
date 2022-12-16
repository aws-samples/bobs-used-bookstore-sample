using Bookstore.Customer.ViewModel.ShoppingCart;
using Bookstore.Domain.Carts;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Customer.Mappers
{
    public static class ShoppingCartItemsToShoppingCartIndexViewModelMapper
    {
        public static ShoppingCartIndexViewModel ToShoppingCartIndexViewModel(this IEnumerable<ShoppingCartItem> shoppingCartItems)
        {
            return new ShoppingCartIndexViewModel
            {
                ShoppingCartItems = shoppingCartItems.Select(c => new ShoppingCartIndexItemViewModel
                {
                    BookId = c.Book.Id,
                    ImageUrl = c.Book.FrontImageUrl,
                    Price = c.Book.Price,
                    BookName = c.Book.Name,
                    ShoppingCartItemId = c.Id,
                    StockLevel = c.Book.Quantity
                }).ToList()
            };
        }
    }
}