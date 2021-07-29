using BobsBookstore.Models.Books;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BobsBookstore.Models.Orders
{
    public class OrderDetail
    {
        [Key]
        public long OrderDetail_Id { get; set; }

        // Many to One Relationship
        public Order Order { get; set; }

        // Many To One Relationship
        public Book Book { get; set; }

        // Many to One Relationship
        public Price Price { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]

        [Column(TypeName = "decimal(18,2)")]

        public decimal OrderDetailPrice { get; set; }

        public int Quantity { get; set; }

        public bool IsRemoved { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
