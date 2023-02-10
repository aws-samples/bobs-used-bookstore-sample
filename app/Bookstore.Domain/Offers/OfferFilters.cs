namespace Bookstore.Domain.Offers
{
    public class OfferFilters
    {
        public string? BookName { get; set; }

        public string? Author { get; set; }

        public int? GenreId { get; set; }

        public int? ConditionId { get; set; }

        public OfferStatus? OfferStatus { get; set; }
    }
}
