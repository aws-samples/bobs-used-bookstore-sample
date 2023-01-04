using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface IFileService
    {
        public Task<string> SaveAsync(IFormFile file);
        
        public Task DeleteAsync(string filePath);
    }
}
