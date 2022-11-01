using System.ComponentModel.DataAnnotations;

namespace Bookstore.Domain
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}