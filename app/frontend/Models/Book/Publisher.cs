using System.ComponentModel.DataAnnotations;

namespace BobBookstore.Models.Book
{
    public class Publisher
    {
        [Key]
        public long Publisher_Id { get; set; }

        public string Name { get; set; }
    }
}
