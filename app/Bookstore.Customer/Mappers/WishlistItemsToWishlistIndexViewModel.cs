using Bookstore.Customer.ViewModel.Wishlist;
using Bookstore.Domain.Carts;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Customer.Mappers
{
    public static class WishlistItemsToWishlistIndexViewModel
    {
        public static WishlistIndexViewModel ToWishlistIndexViewModel(this IEnumerable<ShoppingCartItem> wishlistItems)
        {
            return new WishlistIndexViewModel
            {
                WishlistItems = wishlistItems.Select(x => new WishlistIndexItemViewModel
                {
                    ShoppingCartItemId = x.Id,
                    BookName = x.Book.Name,
                    ImageUrl = x.Book.FrontImageUrl,
                    Price = x.Book.Price
                }).ToList()
            };
        }
    }
}