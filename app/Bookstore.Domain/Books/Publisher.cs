using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Books
{
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Publisher_Id { get; set; }

        public string Name { get; set; }

        [Timestamp] public byte[] RowVersion { get; set; }
    }
}