using Bookstore.Domain.Addresses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AddressRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async Task IAddressRepository.DeleteAsync(string sub, int id)
        {
            var address = await dbContext.Addresses.SingleOrDefaultAsync(x => x.Customer.Sub == sub && x.Id == id);

            if (address == null) return;

            address.IsActive = false;
        }

        async Task<Address> IAddressRepository.GetAsync(string sub, int id)
        {
            return await dbContext.Addresses.SingleOrDefaultAsync(x => x.Customer.Sub == sub && x.Id == id && x.IsActive == true);
        }

        async Task<IEnumerable<Address>> IAddressRepository.ListAsync(string sub)
        {
            return await dbContext.Addresses.Where(x => x.Customer.Sub == sub && x.IsActive == true).ToListAsync();
        }

        async Task IAddressRepository.AddAsync(Address address)
        {
            await dbContext.Addresses.AddAsync(address);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}