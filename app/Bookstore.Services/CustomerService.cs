using Bookstore.Data.Repository.Interface;
using Bookstore.Domain.Customers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface ICustomerService
    {
        Customer Get(int id);

        Customer Get(string sub);

        Task SaveAsync(Customer customer);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> customerRepository;

        public CustomerService(IGenericRepository<Customer> customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public Customer Get(int id)
        {
            return customerRepository.Get(id);
        }

        public Customer Get(string sub)
        {
            return customerRepository.Get2(x => x.Sub == sub).SingleOrDefault();
        }

        public async Task SaveAsync(Customer customer)
        {
            var existingCustomer = customerRepository.Get2(x => x.Sub == customer.Sub).FirstOrDefault();

            if (existingCustomer == null) existingCustomer = new Customer();

            existingCustomer.Sub = customer.Sub;
            existingCustomer.Email = customer.Email;
            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Username = customer.Username;
            existingCustomer.CreatedBy = "System";
            existingCustomer.UpdatedOn = DateTime.UtcNow;

            customerRepository.AddOrUpdate(existingCustomer);

            await customerRepository.SaveAsync();
        }
    }
}