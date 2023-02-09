using System.ComponentModel.DataAnnotations;

namespace Bookstore.Domain
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public string CreatedBy { get; set; } = "System";

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public bool IsNewEntity()
        {
            return Id == 0;
        }
    }
}