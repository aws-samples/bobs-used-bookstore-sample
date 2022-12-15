using Bookstore.Data.Repository.Interface;
using Bookstore.Domain.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface IShoppingCartService
    {
        Task AddAsync(string shoppingCartClientId, int bookId, int quantity);

        Task DeleteShoppingCartItemAsync(string shoppingCartClientId, int id);
        
        IEnumerable<ShoppingCartItem> GetShoppingCartItems(string shoppingCartClientId);
    }

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IGenericRepository<ShoppingCart> shoppingCartRepository;
        private readonly IGenericRepository<ShoppingCartItem> shoppingCartItemRepository;

        public ShoppingCartService(
            IGenericRepository<ShoppingCart> shoppingCartRepository, IGenericRepository<ShoppingCartItem> shoppingCartItemRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.shoppingCartItemRepository = shoppingCartItemRepository;
        }

        public async Task AddAsync(string shoppingCartClientId, int bookId, int quantity)
        {
            var shoppingCart = shoppingCartRepository.Get2(x => x.CorrelationId == shoppingCartClientId).SingleOrDefault();

            if (shoppingCart == null)
            {
                shoppingCart ??= new ShoppingCart { CorrelationId = shoppingCartClientId };

                await shoppingCartRepository.AddAsync(shoppingCart);

                await shoppingCartRepository.SaveAsync();
            }

            var shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = shoppingCart.Id,
                BookId = bookId,
                Quantity = quantity,
                WantToBuy = true
            };

            await shoppingCartItemRepository.AddAsync(shoppingCartItem);

            await shoppingCartItemRepository.SaveAsync();
        }

        public async Task DeleteShoppingCartItemAsync(string shoppingCartClientId, int id)
        {
            var shoppingCartItem = shoppingCartItemRepository.Get2(x => x.ShoppingCart.CorrelationId ==shoppingCartClientId && x.Id == id).SingleOrDefault();

            shoppingCartItemRepository.Remove(shoppingCartItem);

            await shoppingCartItemRepository.SaveAsync();
        }

        public IEnumerable<ShoppingCartItem> GetShoppingCartItems(string shoppingCartClientId)
        {
            return shoppingCartItemRepository.Get2(x => x.ShoppingCart.CorrelationId == shoppingCartClientId, includeProperties: x => x.Book);
        }
    }
}
