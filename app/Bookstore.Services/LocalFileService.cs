using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.Services
{

    public class LocalFileService : IFileService
    {
        private IWebHostEnvironment environment;

        public LocalFileService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public void Delete(string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            if (file == null) return null;

            var uploadFolder = Path.Combine(environment.WebRootPath, "images");
            var filename = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{Path.GetExtension(file.FileName)}";

            if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

            using (var filestream = new FileStream(Path.Combine(uploadFolder, filename), FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(filestream);
                await filestream.FlushAsync();
            }

            return $"/images/{filename}";
        }
    }
}