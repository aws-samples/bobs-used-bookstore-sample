using System.ComponentModel.DataAnnotations;

namespace BobBookstore.Models.Carts
{
    public class Cart
    {
        [Key]
        public string Cart_Id { get; set; }
        public Customer.Customer Customer { get; set; }
        public string IP { get; set; }

    }
}
