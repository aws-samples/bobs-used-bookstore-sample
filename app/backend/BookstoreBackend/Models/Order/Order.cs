using System.ComponentModel.DataAnnotations;
using BookstoreBackend.Models.Customer;

namespace BookstoreBackend.Models.Order
{
    public class Order
    {
        /*
         * Order Model
         * Object reference indicates relationship to other tables
         */

        [Key]
        public long Order_Id { get; set; }

        public double Subtotal { get; set; }

        public double Tax { get; set; }

        public string DeliveryDate { get; set; }

        // One to Many Relationship with OrderStatus Table
        public OrderStatus OrderStatus { get; set; }

        // Many to One Relationship with Customer Table
        public Customer.Customer Customer { get; set; }

        // Many to One Relationship with Address Table 
        public Address Address { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
