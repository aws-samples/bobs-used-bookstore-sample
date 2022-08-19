using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Books
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