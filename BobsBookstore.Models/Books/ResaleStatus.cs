using System.ComponentModel.DataAnnotations;

namespace BobsBookstore.Models.Books
{
    public class ResaleStatus
    {
        [Key]
        public long ResaleStatus_Id { get; set; }

        public string Status { get; set; }
    }
}
