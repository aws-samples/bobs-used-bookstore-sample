using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public class LocalFileService : IFileService
    {
        private readonly IWebHostEnvironment environment;

        public LocalFileService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task DeleteAsync(string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            if (file == null) return null;

            var adminAppImageFolder = Path.Combine(environment.WebRootPath, "images");
            var customerAppImageFolder = adminAppImageFolder.Replace("Bookstore.Admin", "Bookstore.Customer");
            var filename = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{Path.GetExtension(file.FileName)}";

            await SaveAsync(file, adminAppImageFolder, filename);
            await SaveAsync(file, customerAppImageFolder, filename);

            return $"/images/{filename}";
        }

        private async Task SaveAsync(IFormFile file, string foldername, string filename)
        {
            if (!Directory.Exists(foldername)) Directory.CreateDirectory(foldername);

            using var filestream = new FileStream(Path.Combine(foldername, filename), FileMode.OpenOrCreate);

            await file.CopyToAsync(filestream);
            await filestream.FlushAsync();
        }
    }
}