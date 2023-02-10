namespace Bookstore.Domain.Carts
{
    public class ShoppingCart : Entity
    {
        public List<ShoppingCartItem> ShoppingCartItems { get; private set; } = new();

        public string CorrelationId { get; set; }

        public ShoppingCart(string correlationId)
        {
            CorrelationId = correlationId;
        }

        public IEnumerable<ShoppingCartItem> GetShoppingCartItems(ShoppingCartItemFilter filter)
        {
            return filter == ShoppingCartItemFilter.IncludeOutOfStockItems ?
                ShoppingCartItems.Where(x => x.WantToBuy) :
                ShoppingCartItems.Where(x => x.WantToBuy && x.Book.Quantity > 0);
        }

        public IEnumerable<ShoppingCartItem> GetWishListItems()
        {
            return ShoppingCartItems.Where(x => x.WantToBuy == false);
        }

        public void AddItemToShoppingCart(int bookId, int quantity)
        {
            ShoppingCartItems.Add(new ShoppingCartItem(this, bookId, quantity, true));
        }

        public void AddItemToWishlist(int bookId)
        {
            ShoppingCartItems.Add(new ShoppingCartItem(this, bookId, 1, false));
        }

        public void MoveWishListItemToShoppingCart(int shoppingCartItemId)
        {
            var wishListItem = ShoppingCartItems.SingleOrDefault(x => x.Id == shoppingCartItemId);

            if (wishListItem == null) return;

            wishListItem.WantToBuy = true;
        }

        public void RemoveShoppingCartItemById(int shoppingCartItemId)
        {
            var shoppingCartItem = ShoppingCartItems.Single(x => x.Id == shoppingCartItemId);

            ShoppingCartItems.Remove(shoppingCartItem);
        }

        public decimal GetSubTotal(ShoppingCartItemFilter filter)
        {
            return GetShoppingCartItems(filter).Sum(x => x.Book.Price);
        }
    }

    public enum ShoppingCartItemFilter
    {
        IncludeOutOfStockItems,
        ExcludeOutOfStockItems
    }
}