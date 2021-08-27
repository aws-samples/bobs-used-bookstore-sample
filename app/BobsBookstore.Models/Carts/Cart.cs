using BobsBookstore.Models.Customers;
using System.ComponentModel.DataAnnotations;

namespace BobsBookstore.Models.Carts
{
    public class Cart
    {
        [Key]
        public string Cart_Id { get; set; }
        public Customer Customer { get; set; }
        public string IP { get; set; }
    }
}
