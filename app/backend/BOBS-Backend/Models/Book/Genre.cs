using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreBackend.Models.Book
{
    public class Genre
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Genre_Id { get; set; }

        public string Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
