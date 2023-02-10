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
        }
    }
}