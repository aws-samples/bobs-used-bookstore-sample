using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BobsBookstore.Models.Books
{
    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Book_Id { get; set; }

        public string Author { get; set; }

        public Publisher Publisher { get; set; }


        public string ISBN { get; set; }


        public Type Type { get; set; }

        public string Name { get; set; }

        public Genre Genre { get; set; }

        public string Front_Url { get; set; }

        public string Back_Url { get; set; }

        public string Left_Url { get; set; }
        public string Right_Url { get; set; }

        public string AudioBook_Url { get; set; }

        public string Summary { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
