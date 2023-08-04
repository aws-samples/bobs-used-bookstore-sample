namespace Bookstore.Domain.Customers
{
    public interface ICustomerRepository
    {
        internal protected Task<Customer> GetAsync(int id);

        internal protected Task<Customer> GetAsync(string sub);

        internal protected Task AddAsync(Customer customer);

        internal protected Task SaveChangesAsync();
    }
}