using System.ComponentModel.DataAnnotations;

namespace BookstoreBackend.Models.Order
{
    public class OrderStatus
    {
        /*
         * OrderStatus Model 
         */

        [Key]
        public long OrderStatus_Id { get; set; }

        public string Status { get; set; }

        public int position { get; set; }

    }
}
