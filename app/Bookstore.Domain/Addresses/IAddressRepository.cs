namespace Bookstore.Domain.Addresses
{
    public interface IAddressRepository
    {
        internal protected Task<Address> GetAsync(string sub, int id);

        internal protected Task<IEnumerable<Address>> ListAsync(string sub);

        internal protected Task AddAsync(Address address);

        internal protected Task DeleteAsync(string sub, int id);

        internal protected Task SaveChangesAsync();
    }
}