namespace Bookstore.Domain.Carts
{
    public record AddToShoppingCartDto(string CorrelationId, int BookId, int Quantity);

    public record AddToWishlistDto(string CorrelationId, int BookId);

    public record MoveWishlistItemToShoppingCartDto(string CorrelationId, int ShoppingCartItemId);

    public record MoveAllWishlistItemsToShoppingCartDto(string CorrelationId);

    public record DeleteShoppingCartItemDto(string CorrelationId, int ShoppingCartItemId);
}
