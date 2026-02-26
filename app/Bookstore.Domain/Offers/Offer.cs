using Bookstore.Domain.Customers;
using Bookstore.Domain.ReferenceData;

namespace Bookstore.Domain.Offers
{
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

        public string Author { get; set; }

        public string ISBN { get; set; }

        public string BookName { get; set; }

        public string? FrontUrl { get; set; }

        public ReferenceDataItem Genre { get; set; }
        public int GenreId { get; set; }

        public ReferenceDataItem Condition { get; set; }
        public int ConditionId { get; set; }

        public ReferenceDataItem Publisher { get; set; }
        public int PublisherId { get; set; }

        public ReferenceDataItem BookType { get; set; }
        public int BookTypeId { get; set; }

        public string? Summary { get; set; }

        public OfferStatus OfferStatus { get; set; } = OfferStatus.PendingApproval;

        public string? Comment { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        public decimal BookPrice { get; set; }
    }
}