using Bookstore.Domain.Customers;

namespace Bookstore.Domain.Offers
{
    public class Offer : Entity
    {
        public string Author { get; set; }

        public string ISBN { get; set; }

        public string BookName { get; set; }

        public string? FrontUrl { get; set; }

        public string GenreName { get; set; }

        public string? BackUrl { get; set; }

        public string? LeftUrl { get; set; }

        public string? RightUrl { get; set; }

        public string? AudioBookUrl { get; set; }

        public string? Summary { get; set; }

        public string PublisherName { get; set; }

        public string TypeName { get; set; }

        public OfferStatus OfferStatus { get; set; }

        public string? Comment { get; set; }

        public Customer Customer { get; set; }

        public decimal BookPrice { get; set; }

        public string ConditionName { get; set; }
    }
}