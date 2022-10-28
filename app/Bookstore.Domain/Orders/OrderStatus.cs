using System.ComponentModel.DataAnnotations;

namespace Bookstore.Domain.Orders
{
    public class OrderStatus
    {
        [Key] public long OrderStatus_Id { get; set; }

        public string Status { get; set; }

        public int Position { get; set; }
    }
}