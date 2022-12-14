using Bookstore.Domain.Customers;
using Bookstore.Domain.ReferenceData;

namespace Bookstore.Domain.Offers
{
    public class Offer : Entity
    {
        public string Author { get; set; }

        public string ISBN { get; set; }

        public string BookName { get; set; }

        public string? FrontUrl { get; set; }

        public ReferenceDataItem? Genre { get; set; }
        public int? GenreId { get; set; }

        public ReferenceDataItem? Condition { get; set; }
        public int? ConditionId { get; set; }

        public ReferenceDataItem? Publisher { get; set; }
        public int? PublisherId { get; set; }

        public ReferenceDataItem? BookType { get; set; }
        public int? BookTypeId { get; set; }

        public string? BackUrl { get; set; }

        public string? LeftUrl { get; set; }

        public string? RightUrl { get; set; }

        public string? AudioBookUrl { get; set; }

        public string? Summary { get; set; }

        public OfferStatus OfferStatus { get; set; }

        public string? Comment { get; set; }

        public Customer Customer { get; set; }

        public decimal BookPrice { get; set; }
    }
}