using System.ComponentModel.DataAnnotations;
using DataModels.Customers;

namespace DataModels.Carts
{
    public class Cart
    {
        [Key] public string Cart_Id { get; set; }

        public Customer Customer { get; set; }

        public string IP { get; set; }
    }
}