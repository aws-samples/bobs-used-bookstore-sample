using System.ComponentModel.DataAnnotations;
using BobsBookstore.Models.Books;

namespace BobsBookstore.Models.Carts
{
    public class CartItem
    {
        [Key] public string CartItem_Id { get; set; }

        public Price Price { get; set; }

        public Cart Cart { get; set; }

        public Book Book { get; set; }

        public bool WantToBuy { get; set; }
    }
}