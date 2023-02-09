using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.ViewModel.Wishlist
{
    public class WishlistIndexViewModel
    {
        public List<WishlistIndexItemViewModel> WishlistItems { get; set; } = new List<WishlistIndexItemViewModel>();

        public WishlistIndexViewModel(Domain.Carts.ShoppingCart shoppingCart)
        {
            if (shoppingCart == null) return;

            WishlistItems = shoppingCart
                .GetWishListItems()
                .Select(x => new WishlistIndexItemViewModel
                {
                    ShoppingCartItemId = x.Id,
                    BookName = x.Book.Name,
                    ImageUrl = x.Book.CoverImageUrl,
                    Price = x.Book.Price
                }).ToList();
        }
    }

    public class WishlistIndexItemViewModel
    {
        public int ShoppingCartItemId { get; set; }

        public string BookName { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }
    }
}
