using Bookstore.Domain.Carts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ShoppingCartRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async Task IShoppingCartRepository.AddAsync(ShoppingCart shoppingCart)
        {
            await dbContext.ShoppingCart.AddAsync(shoppingCart);
        }

        async Task<ShoppingCart> IShoppingCartRepository.GetAsync(string correlationId)
        {
            return await dbContext.ShoppingCart
                .Include(x => x.ShoppingCartItems)
                .ThenInclude(x => x.Book)
                .SingleOrDefaultAsync(x => x.CorrelationId == correlationId);
        }

        async Task IShoppingCartRepository.SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
