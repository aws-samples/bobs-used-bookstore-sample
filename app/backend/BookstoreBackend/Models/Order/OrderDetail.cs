using System;
using System.ComponentModel.DataAnnotations;
using BookstoreBackend.Models.Book;

namespace BookstoreBackend.Models.Order
{
    public class OrderDetail
    {
        /*
         * OrderDetail Model
         * Object reference indicates relationship to other Tables
         */
        [Key]
        public long OrderDetail_Id { get; set; }

        // Many to One Relationship
        public Order Order { get; set; }

        // Many To One Relationship
        public Book.Book Book { get; set; }

        // Many to One Relationship
        public Price Price { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public double price { get; set; }

        public int quantity { get; set; }

        public bool IsRemoved { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
