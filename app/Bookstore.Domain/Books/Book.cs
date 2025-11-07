using Bookstore.Domain.ReferenceData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Books
{
    [Table("book", Schema = "bobsusedbookstore_dbo")]
    public class Book : Entity
    {
        public const int LowBookThreshold = 5;

        public Book(
            string name, 
            string author, 
            string ISBN, 
            int publisherId, 
            int bookTypeId, 
            int genreId,
            int conditionId,
            decimal price,
            int quantity, 
            int? year = null,
            string? summary = null,
            string? coverImageUrl = null)
        {
            Name = name;
            Author = author;
            this.ISBN = ISBN;
            PublisherId = publisherId;
            BookTypeId = bookTypeId;
            GenreId = genreId;
            ConditionId = conditionId;
            Price = price;
            Quantity = quantity;
            Year = year;
            Summary = summary;
            CoverImageUrl = coverImageUrl;
        }

        [Column("name")]
        public string Name { get; set; }

        [Column("author")]
        public string Author { get; set; }

        [Column("year")]
        public int? Year { get; set; }

        [Column("isbn")]
        public string ISBN { get; set; }

        public ReferenceDataItem Publisher { get; set; } = null!;
        [Column("publisherid")]
        public int PublisherId { get; set; }

        public ReferenceDataItem BookType { get; set; } = null!;
        [Column("booktypeid")]
        public int BookTypeId { get; set; }

        public ReferenceDataItem Genre { get; set; } = null!;
        [Column("genreid")]
        public int GenreId { get; set; }

        public ReferenceDataItem Condition { get; set; } = null!;
        [Column("conditionid")]
        public int ConditionId { get; set; }

        [Column("coverimageurl")]
        public string? CoverImageUrl { get; set; }

        [Column("summary")]
        public string? Summary { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [NotMapped]
        public bool IsInStock => Quantity > 0;

        [NotMapped]
        public bool IsLowInStock => Quantity <= LowBookThreshold;

        public void ReduceStockLevel(int quantity)
        {
            Quantity = Math.Max(Quantity - quantity, 0);
        }
    }
}
