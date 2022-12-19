using Bookstore.Data;
using Bookstore.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface ICustomerService
    {
        Customer Get(int id);

        Customer Get(string sub);

        Address GetAddress(string sub, int id);

        IEnumerable<Address> GetAddresses(string sub);

        Task SaveAddressAsync(Address address, string sub);

        Task SaveCustomerAsync(Customer customer);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> customerRepository;
        private readonly IGenericRepository<Address> addressRepository;

        public CustomerService(IGenericRepository<Customer> customerRepository, IGenericRepository<Address> AddressRepository)
        {
            this.customerRepository = customerRepository;
            addressRepository = AddressRepository;
        }

        public Customer Get(int id)
        {
            return customerRepository.Get(id);
        }

        public Customer Get(string sub)
        {
            return customerRepository.Get(x => x.Sub == sub).SingleOrDefault();
        }

        public Address GetAddress(string sub, int id)
        {
            return addressRepository.Get(x => x.Customer.Sub == sub && x.Id == id).SingleOrDefault();
        }

        public IEnumerable<Address> GetAddresses(string sub)
        {
            return addressRepository.Get(x => x.Customer.Sub == sub);
        }

        public async Task SaveAddressAsync(Address address, string sub)
        {
            var customer = customerRepository.Get(x => x.Sub == sub).Single();

            address.Customer = customer;

            addressRepository.AddOrUpdate(address);

            await addressRepository.SaveAsync();
        }

        public async Task SaveCustomerAsync(Customer customer)
        {
            var existingCustomer = customerRepository.Get(x => x.Sub == customer.Sub).FirstOrDefault();

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