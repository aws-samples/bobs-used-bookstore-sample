namespace Bookstore.Domain.Books
{
    public record CreateBookDto(
        string Name,
        string Author,
        int BookTypeId,
        int ConditionId,
        int GenreId,
        int PublisherId,
        int? Year,
        string ISBN,
        string Summary,
        decimal Price,
        int Quantity,
        Stream CoverImage,
        string CoverImageFileName);

    public record UpdateBookDto(
        int BookId,
        string Name,
        string Author,
        int BookTypeId,
        int ConditionId,
        int GenreId,
        int PublisherId,
        int? Year,
        string ISBN,
        string Summary,
        decimal Price,
        int Quantity,
        Stream CoverImage,
        string CoverImageFileName);
}