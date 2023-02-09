using Bookstore.Domain.Books;

namespace Bookstore.Domain.Carts
{
    public interface IShoppingCartService
    {
        Task AddToShoppingCartAsync(string shoppingCartCorrelationId, int bookId, int quantity);

        Task AddToWishlistAsync(string shoppingCartCorrelationId, int bookId);

        Task DeleteShoppingCartItemAsync(string shoppingCartCorrelationId, int id);

        Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartCorrelationId);

        Task MoveAllWishlistItemsToShoppingCartAsync(string shoppingCartCorrelationId);

        Task MoveWishlistItemToShoppingCartAsync(string shoppingCartCorrelationId, int shoppingCartItemId);
    }

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IBookRepository bookRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IBookRepository bookRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.bookRepository = bookRepository;
        }

        public async Task AddToShoppingCartAsync(string shoppingCartCorrelationId, int bookId, int quantity)
        {
            await AddToShoppingCartAsync(shoppingCartCorrelationId, bookId, quantity, true);
        }

        public async Task AddToWishlistAsync(string shoppingCartCorrelationId, int bookId)
        {
            await AddToShoppingCartAsync(shoppingCartCorrelationId, bookId, 1, false);
        }

        private async Task AddToShoppingCartAsync(string shoppingCartCorrelationId, int bookId, int quantity, bool wantToBuy)
        {
            var shoppingCart = await shoppingCartRepository.GetAsync(shoppingCartCorrelationId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart { CorrelationId = shoppingCartCorrelationId };

                await shoppingCartRepository.AddAsync(shoppingCart);
            }

            if (wantToBuy)
            {
                shoppingCart.AddItemToShoppingCart(bookId, quantity);
            }
            else
            {
                shoppingCart.AddItemToWishList(bookId);
            }

            await shoppingCartRepository.SaveChangesAsync();
        }

        public async Task DeleteShoppingCartItemAsync(string shoppingCartCorrelationId, int id)
        {
            var shoppingCart = await shoppingCartRepository.GetAsync(shoppingCartCorrelationId);

            shoppingCart.RemoveShoppingCartItemById(id);

            await shoppingCartRepository.SaveChangesAsync();
        }

        public async Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartCorrelationId)
        {
            return await shoppingCartRepository.GetAsync(shoppingCartCorrelationId);
        }

        public async Task MoveWishlistItemToShoppingCartAsync(string shoppingCartCorrelationId, int shoppingCartItemId)
        {
            var shoppingCart = await shoppingCartRepository.GetAsync(shoppingCartCorrelationId);

            shoppingCart.MoveWishListItemToShoppingCart(shoppingCartItemId);

            await shoppingCartRepository.SaveChangesAsync();
        }

        public async Task MoveAllWishlistItemsToShoppingCartAsync(string shoppingCartCorrelationId)
        {
            var shoppingCart = await shoppingCartRepository.GetAsync(shoppingCartCorrelationId);

            foreach (var wishListItem in shoppingCart.GetWishListItems())
            {
                shoppingCart.MoveWishListItemToShoppingCart(wishListItem.Id);
            }

            await shoppingCartRepository.SaveChangesAsync();
        }
    }
}