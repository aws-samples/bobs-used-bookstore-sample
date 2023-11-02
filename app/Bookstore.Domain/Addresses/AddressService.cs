using Bookstore.Domain.Customers;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bookstore.Domain.Addresses
{
    public interface IAddressService
    {
        Task<Address> GetAddressAsync(string sub, int id);

        Task<IEnumerable<Address>> GetAddressesAsync(string sub);

        Task DeleteAddressAsync(DeleteAddressDto deleteAddressDto);

        Task CreateAddressAsync(CreateAddressDto createAddressDto);

        Task UpdateAddressAsync(UpdateAddressDto updateAddressDto);
    }

    public class AddressService : IAddressService
    {
        private readonly IAddressRepository addressRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<AddressService> logger;

        public AddressService(IAddressRepository addressRepository, ICustomerRepository customerRepository, ILoggerFactory logger)
        {
            this.addressRepository = addressRepository;
            this.customerRepository = customerRepository;
            this.logger = logger.CreateLogger<AddressService>();
        }

        public async Task<Address> GetAddressAsync(string sub, int id)
        {
            logger.LogInformation("Retrieving address for ID: {id}", id);
            return await addressRepository.GetAsync(sub, id);
        }

        public async Task<IEnumerable<Address>> GetAddressesAsync(string sub)
        {
            return await addressRepository.ListAsync(sub);
        }

        public async Task CreateAddressAsync(CreateAddressDto dto)
        {
            var customer = await customerRepository.GetAsync(dto.CustomerSub);
            var address = new Address(customer, dto.AddressLine1, dto.AddressLine2, dto.City, dto.State, dto.Country, dto.ZipCode);

            await addressRepository.AddAsync(address);

            await addressRepository.SaveChangesAsync();

            logger.LogInformation("Saved a new address for the Customer with Id {id}. Address : {address}", 
                                   customer.Id,
                                   JsonSerializer.Serialize(new { address.AddressLine1, address.AddressLine2, address.City, address.State, address.Country, address.ZipCode }));
        }

        public async Task UpdateAddressAsync(UpdateAddressDto dto)
        {
            var address = await addressRepository.GetAsync(dto.CustomerSub, dto.AddressId);

            address.AddressLine1 = dto.AddressLine1;
            address.AddressLine2 = dto.AddressLine2;
            address.City = dto.City;
            address.State = dto.State;
            address.Country = dto.Country;
            address.ZipCode = dto.ZipCode;

            await addressRepository.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(DeleteAddressDto dto)
        {
            await addressRepository.DeleteAsync(dto.CustomerSub, dto.AddressId);

            await addressRepository.SaveChangesAsync();
        }
    }
}