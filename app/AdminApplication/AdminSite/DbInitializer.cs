using Amazon.S3;
using Amazon.S3.Transfer;
using DataAccess.Data;
using DataModels.Books;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace AdminSite
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, string bucketName, string cdnDomain, string path)
        {
            context.Database.EnsureCreated();
            System.Diagnostics.Trace.WriteLine("message");

           /* if (context.Book.Any())
            {
                return;
            }*/
           /* var books = new Book[]
            {
                new Book{Author = "Test Lastname", Publisher = new Publisher{Name = "Stupore Bookhouse" }, ISBN = "6775546678", Type = new Type{TypeName = "AudioBook" }, Name = "Stupore Cries", Genre = new Genre{ Name = "Horror"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "stupore.jpg") }
            };

            foreach (Book s in books)
            {
                context.Book.Add(s);
            }
            context.SaveChanges();*/
        }

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
