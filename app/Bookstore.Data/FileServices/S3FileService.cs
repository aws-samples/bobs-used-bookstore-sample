using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Bookstore.Domain;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.Data.FileServices
{
    public class S3FileService : IFileService
    {
        private readonly IConfiguration configuration;
        private readonly TransferUtility transferUtility;

        public S3FileService(IConfiguration configuration, IAmazonS3 s3Client)
        {
            this.configuration = configuration;
            transferUtility = new TransferUtility(s3Client);
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

        public async Task<string> SaveAsync(Stream contents, string filename)
        {
            if (contents == null) return null;

            var bucketName = configuration["AWS:BucketName"];
            var uniqueFilename = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{Path.GetExtension(filename)}";
            var cloudFrontDomain = configuration["AWS:CloudFrontDomain"];

            var request = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                InputStream = contents,
                Key = uniqueFilename
            };

            await transferUtility.UploadAsync(request);

            return $"{cloudFrontDomain}/{uniqueFilename}";
        }
    }
}
