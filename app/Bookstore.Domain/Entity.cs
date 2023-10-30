using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bookstore.Domain
{
    public abstract class Entity
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public string CreatedBy { get; set; } = "System";

        [JsonIgnore]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public bool IsNewEntity()
        {
            return Id == 0;
        }
    }
}