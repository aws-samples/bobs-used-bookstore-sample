using Bookstore.Domain.Customers;
using Bookstore.Domain.ReferenceData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Offers
{
    [Table("offer", Schema = "public")]
    public class Offer : Entity
    {
        public Offer(
            int customerId,
            string bookName,
            string author,
            string ISBN,
            int bookTypeId,
            int conditionId,
            int genreId,
            int publisherId,
            decimal bookPrice)
        {
            CustomerId = customerId;
            BookName = bookName;
            Author = author;
            this.ISBN = ISBN;
            BookTypeId = bookTypeId;
            ConditionId = conditionId;
            GenreId = genreId;
            PublisherId = publisherId;
            BookPrice = bookPrice;
        }

        [Column("author")]
        public string Author { get; set; }

        [Column("isbn")]
        public string ISBN { get; set; }

        [Column("book_name")]
        public string BookName { get; set; }

        [Column("front_url")]
        public string? FrontUrl { get; set; }

        public ReferenceDataItem Genre { get; set; }
        [Column("genre_id")]
        public int GenreId { get; set; }

        public ReferenceDataItem Condition { get; set; }
        [Column("condition_id")]
        public int ConditionId { get; set; }

        public ReferenceDataItem Publisher { get; set; }
        [Column("publisher_id")]
        public int PublisherId { get; set; }

        public ReferenceDataItem BookType { get; set; }
        [Column("book_type_id")]
        public int BookTypeId { get; set; }

        [Column("summary")]
        public string? Summary { get; set; }

        [Column("offer_status")]
        public OfferStatus OfferStatus { get; set; } = OfferStatus.PendingApproval;

        [Column("comment")]
        public string? Comment { get; set; }

        public Customer Customer { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("book_price")]
        public decimal BookPrice { get; set; }
    }
}