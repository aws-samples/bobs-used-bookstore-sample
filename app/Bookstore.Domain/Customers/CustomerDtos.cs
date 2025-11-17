namespace Bookstore.Domain.Customers
{
    public record CreateOrUpdateCustomerDto(
        string CustomerSub,
        string Username,
        string FirstName,
        string LastName);
}
