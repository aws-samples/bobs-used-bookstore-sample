using Bookstore.Domain.ReferenceData;

namespace Bookstore.Domain.Books
{
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

        public string Name { get; set; }

        public string Author { get; set; }

        public int? Year { get; set; }

        public string ISBN { get; set; }

        public ReferenceDataItem Publisher { get; set; }
        public int PublisherId { get; set; }

        public ReferenceDataItem BookType { get; set; }
        public int BookTypeId { get; set; }

        public ReferenceDataItem Genre { get; set; }
        public int GenreId { get; set; }

        public ReferenceDataItem Condition { get; set; }
        public int ConditionId { get; set; }

        public string? CoverImageUrl { get; set; }

        public string? Summary { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public bool IsInStock => Quantity > 0;

        public bool IsLowInStock => Quantity <= LowBookThreshold;

        public void ReduceStockLevel(int quantity)
        {
            Quantity = Math.Max(Quantity - quantity, 0);
        }
    }
}