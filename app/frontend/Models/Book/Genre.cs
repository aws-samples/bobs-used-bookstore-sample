using System.ComponentModel.DataAnnotations;

namespace BobBookstore.Models.Book
{
    public class Genre
    {
        [Key]
        public long Genre_Id { get; set; }

        public string Name { get; set; }
    }
}
