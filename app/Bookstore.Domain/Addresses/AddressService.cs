using Bookstore.Domain.Customers;

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

        public AddressService(IAddressRepository addressRepository, ICustomerRepository customerRepository)
        {
            this.addressRepository = addressRepository;
            this.customerRepository = customerRepository;
        }

        public async Task<Address> GetAddressAsync(string sub, int id)
        {
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