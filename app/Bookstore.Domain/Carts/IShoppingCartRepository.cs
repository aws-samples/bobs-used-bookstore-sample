namespace Bookstore.Domain.Carts
{
    public interface IShoppingCartRepository
    {
        internal protected Task AddAsync(ShoppingCart shoppingCart);

        internal protected Task<ShoppingCart> GetAsync(string correlationId);

        internal protected Task SaveChangesAsync();
    }
}
