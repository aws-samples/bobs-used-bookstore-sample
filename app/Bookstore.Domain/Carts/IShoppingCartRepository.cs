namespace Bookstore.Domain.Carts
{
    public interface IShoppingCartRepository
    {
        protected internal Task AddAsync(ShoppingCart shoppingCart);

        protected internal Task<ShoppingCart> GetAsync(string correlationId);

        protected internal Task SaveChangesAsync();
    }
}
