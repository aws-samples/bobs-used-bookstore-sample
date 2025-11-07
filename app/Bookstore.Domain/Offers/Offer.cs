using Bookstore.Domain.Customers;
using Bookstore.Domain.ReferenceData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Offers
{
    [Table("offer", Schema = "bobsusedbookstore_dbo")]
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

        [Column("bookname")]
        public string BookName { get; set; }

        [Column("fronturl")]
        public string? FrontUrl { get; set; }

        public ReferenceDataItem Genre { get; set; } = null!;
        [Column("genreid")]
        public int GenreId { get; set; }

        public ReferenceDataItem Condition { get; set; } = null!;
        [Column("conditionid")]
        public int ConditionId { get; set; }

        public ReferenceDataItem Publisher { get; set; } = null!;
        [Column("publisherid")]
        public int PublisherId { get; set; }

        public ReferenceDataItem BookType { get; set; } = null!;
        [Column("booktypeid")]
        public int BookTypeId { get; set; }

        [Column("summary")]
        public string? Summary { get; set; }

        [Column("offerstatus")]
        public OfferStatus OfferStatus { get; set; } = OfferStatus.PendingApproval;

        [Column("comment")]
        public string? Comment { get; set; }

        public Customer Customer { get; set; } = null!;
        [Column("customerid")]
        public int CustomerId { get; set; }

        [Column("bookprice")]
        public decimal BookPrice { get; set; }
    }
}