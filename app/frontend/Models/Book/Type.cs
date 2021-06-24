using System.ComponentModel.DataAnnotations;

namespace BobBookstore.Models.Book
{
    public class Type
    {
        [Key]
        public long Type_Id { get; set; }

        public string TypeName { get; set; }
    }
}
