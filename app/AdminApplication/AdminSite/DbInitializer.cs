using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;

namespace AdminSite
{
    public static class DbInitializer
    {
        public static string uploadToS3(string bucketName, string cdnDomain, string path, string bookCover)
        {
            IAmazonS3 S3Client = new AmazonS3Client();
            var url = "";
            var completePath = Path.Combine(path, "images", bookCover);
            var fileTransferUtility = new TransferUtility(S3Client);
            //var imageFileStream = System.IO.File.OpenRead(path);
            fileTransferUtility.Upload(completePath, bucketName);
            url = string.Concat(cdnDomain, "/", bookCover);

            System.Diagnostics.Trace.WriteLine(url);
            return url;

        }
    }
}
