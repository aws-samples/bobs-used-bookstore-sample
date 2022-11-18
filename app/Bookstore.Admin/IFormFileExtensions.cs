using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.Admin
{
    public static class IFormFileExtensions
    {
        public static async Task<Stream> ToStreamAsync(this IFormFile formFile)
        {
            if (formFile == null) return null;

            var result = new MemoryStream();

            await formFile.CopyToAsync(result);

            return result;
        }
    }
}
