using Bookstore.Domain.Books;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Carts
{
    [Table("shoppingcartitem", Schema = "bobsusedbookstore_dbo")]
    public class ShoppingCartItem : Entity
    {
        // An empty constructor is required by EF Core
        private ShoppingCartItem() { }

        public ShoppingCartItem(ShoppingCart shoppingCart, int bookId, int quantity, int wantToBuy)
        {
            ShoppingCartId = shoppingCart.Id;
            ShoppingCart = shoppingCart;
            BookId = bookId;
            Quantity = quantity;
            WantToBuy = wantToBuy;
        }

        [Column("shoppingcartid")]
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        [Column("bookid")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("wanttobuy")]
        public int WantToBuy { get; set; }
    }
}