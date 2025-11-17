namespace Bookstore.Domain.Carts
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetShoppingCartAsync(string correlationId);

        Task AddToShoppingCartAsync(AddToShoppingCartDto addToShoppingCartDto);

        Task AddToWishlistAsync(AddToWishlistDto addToWishlistDto);

        Task MoveWishlistItemToShoppingCartAsync(MoveWishlistItemToShoppingCartDto moveWishlistItemToShoppingCartDto);

        Task MoveAllWishlistItemsToShoppingCartAsync(MoveAllWishlistItemsToShoppingCartDto moveAllWishlistItemsToShoppingCartDto);

        Task DeleteShoppingCartItemAsync(DeleteShoppingCartItemDto deleteShoppingCartItemDto);
    }

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartCorrelationId)
        {
            return await shoppingCartRepository.GetAsync(shoppingCartCorrelationId);
        }

        public async Task AddToShoppingCartAsync(AddToShoppingCartDto dto)
        {
            await AddToShoppingCartAsync(dto.CorrelationId, dto.BookId, dto.Quantity, true);
        }

        public async Task AddToWishlistAsync(AddToWishlistDto dto)
        {
            await AddToShoppingCartAsync(dto.CorrelationId, dto.BookId, 1, false);
        }

        private async Task AddToShoppingCartAsync(string correlationId, int bookId, int quantity, bool wantToBuy)
        {
            var shoppingCart = await shoppingCartRepository.GetAsync(correlationId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart(correlationId);

                await shoppingCartRepository.AddAsync(shoppingCart);
            }

            if (wantToBuy)
            {
                shoppingCart.AddItemToShoppingCart(bookId, quantity);
            }
            else
            {
                shoppingCart.AddItemToWishlist(bookId);
            }

            await shoppingCartRepository.SaveChangesAsync();
        }

        public async Task MoveWishlistItemToShoppingCartAsync(MoveWishlistItemToShoppingCartDto dto)
        {
            var shoppingCart = await shoppingCartRepository.GetAsync(dto.CorrelationId);

            shoppingCart.MoveWishListItemToShoppingCart(dto.ShoppingCartItemId);

            await shoppingCartRepository.SaveChangesAsync();
        }

        public async Task MoveAllWishlistItemsToShoppingCartAsync(MoveAllWishlistItemsToShoppingCartDto dto)
        {
            var shoppingCart = await shoppingCartRepository.GetAsync(dto.CorrelationId);

                if (shoppingCart == null) return;

            foreach (var wishListItem in shoppingCart.GetWishListItems())
            {
                shoppingCart.MoveWishListItemToShoppingCart(wishListItem.Id);
            }

            await shoppingCartRepository.SaveChangesAsync();
        }

        public async Task DeleteShoppingCartItemAsync(DeleteShoppingCartItemDto dto)
        {
            var shoppingCart = await shoppingCartRepository.GetAsync(dto.CorrelationId);

            shoppingCart.RemoveShoppingCartItemById(dto.ShoppingCartItemId);

            await shoppingCartRepository.SaveChangesAsync();
        }
    }
}