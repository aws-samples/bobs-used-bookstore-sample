using Bookstore.Domain;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.Data.FileServices
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

        public async Task<string> SaveAsync(Stream file, string filename)
        {
            if (file == null) return null;

            var imageFolder = Path.Combine(webRootPath, "images/coverimages");
            var uniqueFilename = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{Path.GetExtension(filename)}";

            if (!Directory.Exists(imageFolder)) Directory.CreateDirectory(imageFolder);

            using var filestream = new FileStream(Path.Combine(imageFolder, uniqueFilename), FileMode.OpenOrCreate);

            await file.CopyToAsync(filestream);

            await filestream.FlushAsync();

            return $"/images/coverimages/{uniqueFilename}";
        }
    }
}