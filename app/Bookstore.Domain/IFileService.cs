namespace Bookstore.Domain
{
    public interface IFileService
    {
        public Task<string> SaveAsync(Stream? contents, string? filename);

        public Task DeleteAsync(string? filePath);
    }
}
