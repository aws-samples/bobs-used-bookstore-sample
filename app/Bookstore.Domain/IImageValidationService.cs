namespace Bookstore.Domain
{
    public interface IImageValidationService
    {
        public Task<bool> IsSafeAsync(Stream? image);
    }
}