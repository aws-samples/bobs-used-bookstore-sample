using System.ComponentModel.DataAnnotations;

namespace BobBookstore.Models.Order
{
    public class OrderStatus
    {
        [Key]
        public long OrderStatus_Id { get; set; }

        public string Status { get; set; }

        public int position { get; set; }
    }
}
