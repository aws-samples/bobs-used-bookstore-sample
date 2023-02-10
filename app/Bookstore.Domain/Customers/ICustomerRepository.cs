namespace Bookstore.Domain.Customers
{
    public interface ICustomerRepository
    {
        protected internal Task<Customer> GetAsync(int id);

        protected internal Task<Customer> GetAsync(string sub);

        protected internal Task AddAsync(Customer customer);

        protected internal Task SaveChangesAsync();
    }
}