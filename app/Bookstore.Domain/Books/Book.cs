using Bookstore.Domain.ReferenceData;

namespace Bookstore.Domain.Books
{
    public class Book : Entity
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public int? Year { get; set; }

        public string? ISBN { get; set; }

        public ReferenceDataItem? Publisher { get; set; }
        public int? PublisherId { get; set; }

        public ReferenceDataItem? BookType { get; set; }
        public int? BookTypeId { get; set; }

        public ReferenceDataItem? Genre { get; set; }
        public int? GenreId { get; set; }

        public ReferenceDataItem? Condition { get; set; }
        public int? ConditionId { get; set; }

        public string? FrontUrl { get; set; }

        public string? BackUrl { get; set; }

        public string? LeftUrl { get; set; }

        public string? RightUrl { get; set; }

        public string? Summary { get; set; }
    }
}