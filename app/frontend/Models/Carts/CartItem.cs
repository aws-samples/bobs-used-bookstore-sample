using System.ComponentModel.DataAnnotations;
using BobBookstore.Models.Book;

namespace BobBookstore.Models.Carts
{
    public class CartItem
    {
        [Key]
        public string CartItem_Id { get; set; }
        public Price  Price { get; set; }
        public Cart Cart { get; set; }
        public Book.Book Book { get; set; }
        public bool WantToBuy { get; set; }
    }
}
