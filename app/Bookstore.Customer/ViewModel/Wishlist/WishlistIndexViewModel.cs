using Microsoft.Build.Evaluation;
using System.Collections.Generic;

namespace Bookstore.Customer.ViewModel.Wishlist
{
    public class WishlistIndexViewModel
    {
        public List<WishlistIndexItemViewModel> WishlistItems { get; set; } = new List<WishlistIndexItemViewModel>();
    }

    public class WishlistIndexItemViewModel
    {
        public int ShoppingCartItemId { get; set; }

        public string BookName { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }
    }
}
