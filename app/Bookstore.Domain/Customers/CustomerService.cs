using Bookstore.Domain.Carts;
using Microsoft.Extensions.Logging;

namespace Bookstore.Domain.Customers
{
    public interface ICustomerService
    {
        Task<Customer> GetAsync(int id);

        Task<Customer> GetAsync(string sub);

        Task CreateOrUpdateCustomerAsync(CreateOrUpdateCustomerDto createOrUpdateCustomerDto);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<CustomerService> logger;

        public CustomerService(ICustomerRepository customerRepository, ILoggerFactory logger)
        {
            this.customerRepository = customerRepository;
            this.logger = logger.CreateLogger<CustomerService>();
        }

        public async Task<Customer> GetAsync(int id)
        {
            return await customerRepository.GetAsync(id);
        }

        public async Task<Customer> GetAsync(string sub)
        {
            return await customerRepository.GetAsync(sub);
        }
       
        public async Task CreateOrUpdateCustomerAsync(CreateOrUpdateCustomerDto dto)
        {
            var existingCustomer = await customerRepository.GetAsync(dto.CustomerSub);

            if (existingCustomer == null)
            {
                existingCustomer = new Customer();

                await customerRepository.AddAsync(existingCustomer);
            }

            existingCustomer.Sub = dto.CustomerSub;
            existingCustomer.Username = dto.Username;
            existingCustomer.FirstName = dto.FirstName;
            existingCustomer.LastName = dto.LastName;
            existingCustomer.UpdatedOn = DateTime.UtcNow;

            await customerRepository.SaveChangesAsync();

            logger.LogInformation("Updated customer information for the customer: {firstname} {lastname}", dto.FirstName, dto.LastName);
        }
    }
}