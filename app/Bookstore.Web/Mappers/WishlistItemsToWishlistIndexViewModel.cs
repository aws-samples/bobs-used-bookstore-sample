using Bookstore.Domain.Carts;
using Bookstore.Web.ViewModel.Wishlist;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.Mappers
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
                    ImageUrl = x.Book.CoverImageUrl,
                    Price = x.Book.Price
                }).ToList()
            };
        }
    }
}