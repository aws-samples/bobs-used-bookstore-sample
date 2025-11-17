namespace Bookstore.Domain.Addresses
{    
    public record CreateAddressDto(
        string AddressLine1,
        string? AddressLine2,
        string City,
        string State,
        string Country,
        string ZipCode,
        string CustomerSub);

    public record UpdateAddressDto(
        int AddressId,
        string AddressLine1,
        string? AddressLine2,
        string City,
        string State,
        string Country,
        string ZipCode,
        string CustomerSub);

    public record DeleteAddressDto(
        int AddressId,
        string CustomerSub);
}