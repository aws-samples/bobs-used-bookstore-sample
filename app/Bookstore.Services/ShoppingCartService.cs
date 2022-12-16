using Bookstore.Data.Repository.Interface;
using Bookstore.Domain.Carts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface IShoppingCartService
    {
        Task AddToShoppingCartAsync(string shoppingCartCorrelationId, int bookId, int quantity);

        Task AddToWishlistAsync(string shoppingCartCorrelationId, int bookId);

        Task DeleteShoppingCartItemAsync(string shoppingCartCorrelationId, int id);
        
        IEnumerable<ShoppingCartItem> GetShoppingCartItems(string shoppingCartCorrelationId);
        
        IEnumerable<ShoppingCartItem> GetWishlistItems(string shoppingCartCorrelationId);

        Task MoveAllWishlistItemsToShoppingCartAsync(string shoppingCartCorrelationId);

        Task MoveWishlistItemToShoppingCartAsync(string shoppingCartCorrelationId, int shoppingCartItemId);
    }

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IGenericRepository<ShoppingCart> shoppingCartRepository;
        private readonly IGenericRepository<ShoppingCartItem> shoppingCartItemRepository;

        public ShoppingCartService(IGenericRepository<ShoppingCart> shoppingCartRepository, IGenericRepository<ShoppingCartItem> shoppingCartItemRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.shoppingCartItemRepository = shoppingCartItemRepository;
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
            var shoppingCart = shoppingCartRepository.Get2(x => x.CorrelationId == shoppingCartCorrelationId).SingleOrDefault();

            if (shoppingCart == null)
            {
                shoppingCart ??= new ShoppingCart { CorrelationId = shoppingCartCorrelationId };

                await shoppingCartRepository.AddAsync(shoppingCart);

                await shoppingCartRepository.SaveAsync();
            }

            var shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = shoppingCart.Id,
                BookId = bookId,
                Quantity = quantity,
                WantToBuy = wantToBuy
            };

            await shoppingCartItemRepository.AddAsync(shoppingCartItem);

            await shoppingCartItemRepository.SaveAsync();
        }

        public async Task DeleteShoppingCartItemAsync(string shoppingCartCorrelationId, int id)
        {
            var shoppingCartItem = shoppingCartItemRepository.Get2(x => x.ShoppingCart.CorrelationId ==shoppingCartCorrelationId && x.Id == id).SingleOrDefault();

            shoppingCartItemRepository.Remove(shoppingCartItem);

            await shoppingCartItemRepository.SaveAsync();
        }

        public IEnumerable<ShoppingCartItem> GetShoppingCartItems(string shoppingCartCorrelationId)
        {
            return shoppingCartItemRepository.Get2(x => x.ShoppingCart.CorrelationId == shoppingCartCorrelationId && x.WantToBuy == true, includeProperties: x => x.Book);
        }

        public IEnumerable<ShoppingCartItem> GetWishlistItems(string shoppingCartCorrelationId)
        {
            return shoppingCartItemRepository.Get2(x => x.ShoppingCart.CorrelationId == shoppingCartCorrelationId && x.WantToBuy == false, includeProperties: x => x.Book);
        }

        public async Task MoveWishlistItemToShoppingCartAsync(string shoppingCartCorrelationId, int shoppingCartItemId)
        {
            var wishlistItem = shoppingCartItemRepository.Get2(x => x.ShoppingCart.CorrelationId == shoppingCartCorrelationId && x.Id == shoppingCartItemId).SingleOrDefault();

            if (wishlistItem == null) return;

            wishlistItem.WantToBuy= true;

            shoppingCartItemRepository.Update(wishlistItem);

            await shoppingCartItemRepository.SaveAsync();
        }

        public async Task MoveAllWishlistItemsToShoppingCartAsync(string shoppingCartCorrelationId)
        {
            var wishlistItems = shoppingCartItemRepository.Get2(x => x.ShoppingCart.CorrelationId == shoppingCartCorrelationId && x.WantToBuy == false);

            wishlistItems.ToList().ForEach(x => {
                x.WantToBuy = true;

                shoppingCartItemRepository.Update(x);
            });

            await shoppingCartItemRepository.SaveAsync();
        }
    }
}
