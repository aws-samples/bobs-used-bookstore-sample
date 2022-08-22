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

            if (context.Book.Any())
            {
                return;
            }
            var types = new Type[] {
                new Type{TypeName = "Hardcover" },
                new Type{TypeName = "Trade Paperback"},
                new Type{TypeName = "Mass-market paperback"}
            };
            foreach (Type t in types)
            {
                context.Type.Add(t);
            }
            context.SaveChanges();

            Type hardcoverType = context.Type.First(a => a.TypeName == "Hardcover");
            Type tradeType = context.Type.First(a => a.TypeName == "Trade Paperback");
            Type massType = context.Type.First(a => a.TypeName == "Mass-market paperback");

            var books = new Book[]
            {
                new Book{Author = "Marley Cobb", Publisher = new Publisher{Name = "The Twisting Memoirs" }, ISBN = "6556784356", Type = hardcoverType, Name = "2020 : The Apocalypse", Genre = new Genre{ Name = "Sci-Fi"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "2020Apocalypse.png") },
                new Book{Author = "Merritt Chambers", Publisher = new Publisher{Name = "Seal Pen Publishing " }, ISBN = "7665438976", Type = hardcoverType, Name = "Children Of Iron", Genre = new Genre{ Name = "Tragedy"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "childerofiron.png") },
                new Book{Author = "Lee Schuman", Publisher = new Publisher{Name = "Witty Tome" }, ISBN = "5442280765", Type = hardcoverType, Name = "Gold In The Dark", Genre = new Genre{ Name = "Fantasy"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "GoldInTheDark.png") },
                new Book{Author = "Vick Hines", Publisher = new Publisher{Name = "Seamark Publishing" }, ISBN = "4556789542", Type = tradeType, Name = "League Of Smokes", Genre = new Genre{ Name = "Classics"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "leagueOfSmokes.png") },
                new Book{Author = "Bailey Armstrong", Publisher = new Publisher{Name = "Fab Paperback" }, ISBN = "4563358087", Type = tradeType, Name = "Alone With The Stars", Genre = new Genre{ Name = "Action and Adventure"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "AloneWithStars.png") },
                new Book{Author = "Owen Kain", Publisher = new Publisher{Name = "Poetic Publication" }, ISBN = "2354435678", Type = hardcoverType, Name = "The Girl In The Polaroid", Genre = new Genre{ Name = "Crime & Mystery"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "girlinpolaroid.png") },
                new Book{Author = "Adrian Lawrence", Publisher = new Publisher{Name = "All Memoirs" }, ISBN = "6554789632", Type = tradeType, Name = "Nana Lawrence 10001 Jokes", Genre = new Genre{ Name = "Humor and Satire"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "nana.png") },
                new Book{Author = "Harlow Nicholas", Publisher = new Publisher{Name = "Reeve-Chase, Inc" }, ISBN = "4558786554", Type = massType, Name = "My Search For Meaning", Genre = new Genre{ Name = "Biography and Autobiography"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "search.png") }
                /*new Book{Author = "Test Lastname", Publisher = new Publisher{Name = "Stupore Bookhouse" }, ISBN = "4556345790", Type = massType, Name = "Stupore Cries", Genre = new Genre{ Name = "Horror"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "stupore.jpg") },
                new Book{Author = "Test Lastname", Publisher = new Publisher{Name = "Stupore Bookhouse" }, ISBN = "5443567976", Type = massType, Name = "Stupore Cries", Genre = new Genre{ Name = "Horror"}, FrontUrl = uploadToS3(bucketName, cdnDomain, path, "stupore.jpg") }*/
            };

            foreach (Book s in books)
            {
                context.Book.Add(s);
            }
            context.SaveChanges();

            var prices = new Price[]
            {
                new Price{ Book = context.Book.First(a => a.Name == "2020 : The Apocalypse"), Condition = new Condition {ConditionName = "New" }, ItemPrice = 23, Quantity = 100, UpdatedBy = "Admin", Active = true },
                new Price
            {
                Book = context.Book.First(a => a.Name == "Children Of Iron"), Condition = new Condition {ConditionName = "Fine/Like New" }, ItemPrice = 25, Quantity = 100, UpdatedBy = "Admin", Active = true
            },
            new Price
            {
                Book = context.Book.First(a => a.Name == "Gold In The Dark"), Condition = new Condition {ConditionName = "Near Fine" }, ItemPrice = 34, Quantity = 100, UpdatedBy = "Admin", Active = true
            },
             new Price
            {
                Book = context.Book.First(a => a.Name == "League Of Smokes"), Condition = new Condition {ConditionName = "Very Good" }, ItemPrice = 37, Quantity = 100, UpdatedBy = "Admin", Active = true
            },
             new Price
            {
                Book = context.Book.First(a => a.Name == "Alone With The Stars"), Condition = new Condition {ConditionName = "Good" }, ItemPrice = 12, Quantity = 100, UpdatedBy = "Admin", Active = true
            },
             new Price
            {
                Book = context.Book.First(a => a.Name == "The Girl In The Polaroid"), Condition = new Condition {ConditionName = "Fair" }, ItemPrice = 26, Quantity = 100, UpdatedBy = "Admin", Active = true
            },
               new Price
            {
                Book = context.Book.First(a => a.Name == "Nana Lawrence 10001 Jokes"), Condition = new Condition {ConditionName = "Poor" }, ItemPrice = 32, Quantity = 100, UpdatedBy = "Admin", Active = true
            },
                 new Price
            {
                Book = context.Book.First(a => a.Name == "My Search For Meaning"), Condition = new Condition {ConditionName = "Ex-library" }, ItemPrice = 19, Quantity = 100, UpdatedBy = "Admin", Active = true
            }/*
                new Price
            {
                Book = context.Book.First(a => a.Name == "Stupore Cries"), Condition = new Condition {ConditionName = "Book club" }, ItemPrice = 27, Quantity = 100, UpdatedBy = "Admin", Active = true
            },
                 new Price
            {
                Book = context.Book.First(a => a.Name == "Stupore Cries"), Condition = new Condition {ConditionName = "Binding copy" }, ItemPrice = 28, Quantity = 100, UpdatedBy = "Admin", Active = true
            }*/
           };

                foreach (Price p in prices)
            {
                context.Price.Add(p);
            }
            context.SaveChanges();

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
