using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public class S3FileService : IFileService
    {
        private readonly IConfiguration configuration;
        private readonly TransferUtility transferUtility;

        public S3FileService(IConfiguration configuration, IAmazonS3 s3Client)
        {
            this.configuration = configuration;
            this.transferUtility = new TransferUtility(s3Client);
        }

        public async Task DeleteAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;

            var bucketName = configuration["AWS:BucketName"];
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = Path.GetFileName(filePath)
            };

            await transferUtility.S3Client.DeleteObjectAsync(request);
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            if (file == null) return null;

            var bucketName = configuration["AWS:BucketName"];
            var fileName = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{Path.GetExtension(file.FileName)}";
            var cloudFrontDomain = configuration["AWS:CloudFrontDomain"];

            var request = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                InputStream = file.OpenReadStream(),
                Key = fileName
            };

            await transferUtility.UploadAsync(request);

            return $"{cloudFrontDomain}/{fileName}";
        }
    }
}
