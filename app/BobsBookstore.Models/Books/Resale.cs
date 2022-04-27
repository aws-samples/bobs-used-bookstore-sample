using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BobsBookstore.Models.Customers;

namespace BobsBookstore.Models.Books
{
    public class Resale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Resale_Id { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public string BookName { get; set; }

        public string FrontUrl { get; set; }

        public string GenreName { get; set; }

        public string BackUrl { get; set; }

        public string LeftUrl { get; set; }

        public string RightUrl { get; set; }

        public string AudioBookUrl { get; set; }

        public string Summary { get; set; }

        public string PublisherName { get; set; }

        public string TypeName { get; set; }

        public ResaleStatus ResaleStatus { get; set; }

        [Column(TypeName = "varchar(MAX)")] public string Comment { get; set; }

        public Customer Customer { get; set; }

        public decimal BookPrice { get; set; }

        public string ConditionName { get; set; }

        [Timestamp] public byte[] RowVersion { get; set; }
    }
}