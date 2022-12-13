using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public class S3FileService : IFileService
    {
        public void Delete(string filePath)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> SaveAsync(IFormFile file)
        {
            throw new System.NotImplementedException();
        }
    }
}
