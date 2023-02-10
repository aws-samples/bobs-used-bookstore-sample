namespace Bookstore.Domain.Offers
{
    public record CreateOfferDto(
        string CustomerSub,
        string BookName,
        string Author,
        string ISBN,        
        int BookTypeId,
        int ConditionId,
        int GenreId,        
        int PublisherId,
        decimal BookPrice);

    public record UpdateOfferStatusDto(
        int OfferId,
        OfferStatus Status);
}
