namespace Bookstore.Domain
{
    public interface IImageResizeService
    {
        Task<Stream> ResizeImageAsync(Stream image);
    }
}
