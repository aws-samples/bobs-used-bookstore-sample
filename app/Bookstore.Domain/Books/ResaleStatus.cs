using System.ComponentModel.DataAnnotations;

namespace Bookstore.Domain.Books
{
    public class ResaleStatus
    {
        [Key] public long ResaleStatus_Id { get; set; }

        public string Status { get; set; }
    }
}