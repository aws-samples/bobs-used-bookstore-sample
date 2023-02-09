using Bookstore.Domain.Addresses;

namespace Bookstore.Domain.Customers
{
    public interface ICustomerService
    {
        Task<Customer> GetAsync(int id);

        Task<Customer> GetAsync(string sub);

        Task SaveCustomerAsync(Customer customer);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<Customer> GetAsync(int id)
        {
            return await customerRepository.GetAsync(id);
        }

        public async Task<Customer> GetAsync(string sub)
        {
            return await customerRepository.GetAsync(sub);
        }
       
        public async Task SaveCustomerAsync(Customer customer)
        {
            var existingCustomer = await customerRepository.GetAsync(customer.Sub);

            if (existingCustomer == null)
            {
                existingCustomer = new Customer();

                await customerRepository.AddAsync(existingCustomer);
            }

            existingCustomer.Sub = customer.Sub;
            existingCustomer.Email = customer.Email;
            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Username = customer.Username;
            existingCustomer.CreatedBy = "System";
            existingCustomer.UpdatedOn = DateTime.UtcNow;

            await customerRepository.SaveChangesAsync();
        }
    }
}