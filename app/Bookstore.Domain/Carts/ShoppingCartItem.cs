using Bookstore.Domain.Books;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Carts
{
    [Table("shoppingcartitem", Schema = "bobsusedbookstore_dbo")]
    public class ShoppingCartItem : Entity
    {
        // An empty constructor is required by EF Core
        private ShoppingCartItem() { }

        public ShoppingCartItem(ShoppingCart shoppingCart, int bookId, int quantity, bool wantToBuy)
        {
            ShoppingCartId = shoppingCart.Id;
            ShoppingCart = shoppingCart;
            BookId = bookId;
            Quantity = quantity;
            WantToBuy = wantToBuy;
        }

        [Column("shoppingcartid")]
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = null!;

        [Column("bookid")]
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("wanttobuy")]
        public bool WantToBuy { get; set; }
    }
}