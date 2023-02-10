namespace Bookstore.Domain.Addresses
{
    public interface IAddressRepository
    {
        protected internal Task<Address> GetAsync(string sub, int id);

        protected internal Task<IEnumerable<Address>> ListAsync(string sub);

        protected internal Task AddAsync(Address address);

        protected internal Task DeleteAsync(string sub, int id);

        protected internal Task SaveChangesAsync();
    }
}