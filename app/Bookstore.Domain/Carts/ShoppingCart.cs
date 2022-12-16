namespace Bookstore.Domain.Carts
{
    public class ShoppingCart : Entity
    {
        public string CorrelationId { get; set; }

        public IEnumerable<ShoppingCartItem> Items { get; set; }

        public decimal GetSubTotal(ShoppingCartTotalType totalType) 
        { 
            return totalType == ShoppingCartTotalType.IncludeOutOfStockItems ? 
                Items.Sum(x => x.Book.Price) :
                Items.Where(x => x.Book.Quantity > 0).Sum(x => x.Book.Price);
        }
    }

    public enum ShoppingCartTotalType
    {
        IncludeOutOfStockItems,
        ExcludeOutOfStockItems
    }
}