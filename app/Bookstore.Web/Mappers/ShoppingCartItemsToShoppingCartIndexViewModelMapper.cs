using Bookstore.Domain.Carts;
using Bookstore.Web.ViewModel.ShoppingCart;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.Mappers
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
                    ImageUrl = c.Book.CoverImageUrl,
                    Price = c.Book.Price,
                    BookName = c.Book.Name,
                    ShoppingCartItemId = c.Id,
                    StockLevel = c.Book.Quantity
                }).ToList()
            };
        }
    }
}