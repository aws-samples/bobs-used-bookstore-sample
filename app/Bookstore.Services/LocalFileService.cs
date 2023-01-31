using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public class LocalFileService : IFileService
    {
        private readonly string webRootPath;

        public LocalFileService(string webRootPath)
        {
            this.webRootPath = webRootPath;
        }

        public async Task DeleteAsync(string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            if (file == null) return null;

            var imageFolder = Path.Combine(webRootPath, "images/coverimages");
            var filename = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{Path.GetExtension(file.FileName)}";

            if (!Directory.Exists(imageFolder)) Directory.CreateDirectory(imageFolder);

            using var filestream = new FileStream(Path.Combine(imageFolder, filename), FileMode.OpenOrCreate);

            await file.CopyToAsync(filestream);

            await filestream.FlushAsync();

            return $"/images/coverimages/{filename}";
        }
    }
}