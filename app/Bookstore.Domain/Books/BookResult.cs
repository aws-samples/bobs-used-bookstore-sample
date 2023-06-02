namespace Bookstore.Domain.Books
{
    public class BookResult
    {
        public BookResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccess { get; }

        public string? ErrorMessage { get; }
    }
}