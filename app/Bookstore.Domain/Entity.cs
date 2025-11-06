using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain
{
    public abstract class Entity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; } = "System";

        [Column("created_on")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Column("updated_on")]
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        //[Timestamp]
        //public long RowVersion { get; set; }

        public bool IsNewEntity()
        {
            return Id == 0;
        }
    }
}