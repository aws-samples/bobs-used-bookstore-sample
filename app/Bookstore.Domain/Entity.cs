using System;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Domain
{
    public class Entity
    {
        public int Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
