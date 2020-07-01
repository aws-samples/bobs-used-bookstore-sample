using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Transfer;
using BOBS_Backend.Database;
using BOBS_Backend.DataModel;
using BOBS_Backend.Models;
using BOBS_Backend.Models.Book;
using BOBS_Backend.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BOBS_Backend
{
    public class Inventory : IInventory
    {
        public DatabaseContext _context;
        private IHostingEnvironment _env;
       // private readonly BOBS_Backend.ViewModel. _booksview;
        private const string photosBucketName = "bookcoverpictures";
        private const string audioBucketName = "audiosummary";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client;



        public Inventory(DatabaseContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public Book GetBookByID(long Id)
        {
            var book = _context.Book.Find(Id);
            return book;

        }


        public void SaveBook(Book book)
        {
            var books = _context.Book.Find(book.ISBN);
            if (books == null)
            {
                _context.Book.Add(book);
                _context.SaveChanges();
            }

        }

        public void SavePrice(Price price)
        {
            _context.Price.Add(price);
            _context.SaveChanges();

        }

        public void SavePublisherDetails(Publisher publisher)
        {
            var publishers = _context.Publisher.Find(publisher.Publisher_Id);
            if (publishers == null)
            {
                _context.Publisher.Add(publisher);
                _context.SaveChanges();
            }
        }


        public IEnumerable<BookDetails> GetAllBooks()
        {
            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            return booker;
        }

        public async Task<string> UploadtoS3(IFormFile file)
        {
            var filename = file.FileName;
            var dir = _env.ContentRootPath;
            string url = "";

            HashSet<string> _validImageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "jpg", "jpeg", "png", "gif"
        };
           
            
            var fileExt = Path.GetExtension(file.FileName).TrimStart('.');
            // resize the image
            var resizeStream = await ResizeImage(file, fileExt);
            if (_validImageExtensions.Contains(fileExt))
            {
                using (var fileStream = new FileStream(Path.Combine(dir, filename), FileMode.Create, FileAccess.Write))
                {
                    resizeStream.CopyTo(fileStream);
                }
                s3Client = new AmazonS3Client("AKIA6DYNIKQLFG4HU2LU", "4RM8WQL3tH4+c7RgIQ/LPBHqt6ESwleokqsDx1Gf" ,bucketRegion);


                var fileTransferUtility = new TransferUtility(s3Client);

                await fileTransferUtility.UploadAsync(Path.Combine(dir, filename), photosBucketName);

                url = String.Concat("https://ddkveyb1synl2.cloudfront.net/", file.FileName);
                return url;

            }
            return url;
        }
        public async Task<Stream> ResizeImage(IFormFile file,string fileExt)
        {
            // create new memory stream.
            Stream result = new MemoryStream();
            int new_size = 200;
            // create new image variable
            using var img = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
            // set height and width proportional
            var div = img.Width / new_size;
            var hgt = Convert.ToInt32(Math.Round((decimal)(img.Height / div)));
            
            // change size of image
            img.Mutate(x => x.Resize(200, hgt));
            //get the extension encoder
            IImageEncoder encoder = selectEncoder(fileExt);
            img.Save(result, encoder);
            result.Position = 0;
            
            return result;
           
            
        }

        public IImageEncoder selectEncoder(string extension)
        {
            IImageEncoder encoder = null;
            // get the encoder based on file extension
            switch (extension)
            {
                case "png":
                    encoder = new PngEncoder();
                    break;
                case "jpeg":
                    encoder = new JpegEncoder();
                    break;
                case "jpg":
                    encoder = new JpegEncoder();
                    break;
                case "gif":
                    encoder = new GifEncoder();
                    break;
                default:
                    break;
            }
            return encoder;
        }

        public async Task<bool> IsBook(string bucket, string key)
        {
            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(bucketRegion);

            DetectLabelsRequest detectlabelsRequest = new DetectLabelsRequest()
            {
                Image = new Amazon.Rekognition.Model.Image
                {
                    S3Object = new Amazon.Rekognition.Model.S3Object
                    {
                        Name = key,
                        Bucket = bucket
                    },
                },
                MaxLabels = 10,
                MinConfidence = 75F
            };

            int c = 0;
            DetectLabelsResponse detectLabelsResponse = await rekognitionClient.DetectLabelsAsync(detectlabelsRequest);
            foreach (Label label in detectLabelsResponse.Labels)
                if (label.Name == "Book" && label.Confidence >= 90)
                {

                    c++;

                }

            if (c >= 1)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        public async Task<bool> IsImageSafe(string bucket, string key)
        {
            var RekognitionClient = new AmazonRekognitionClient(bucketRegion);
            var response = await RekognitionClient.DetectModerationLabelsAsync(new DetectModerationLabelsRequest
            {
                Image = new Amazon.Rekognition.Model.Image
                {
                    S3Object = new Amazon.Rekognition.Model.S3Object
                    {
                        Bucket = bucket,
                        Name = key
                    }
                }
            });

            if (response.ModerationLabels.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var label in response.ModerationLabels)
                {
                    if (sb.Length > 0)
                        sb.Append(", ");
                    if (!string.IsNullOrEmpty(label.ParentName))
                    {
                        sb.Append(label.ParentName + "/");
                    }

                    sb.Append($"{label.Name}:{label.Confidence}");
                }
            }
            return response.ModerationLabels.Count == 0;
        }

        public IEnumerable<BookDetails> GetRequestedBooks(string BookName , Publisher Publisher , Condition BookCondition , BOBS_Backend.Models.Book.Type type)
        {

            var books = _context.Book.Where(books => books.Name.Contains(BookName));
            if (BookName is null)
            {
              
                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id join publish in _context.Publisher on booke.Publisher.Publisher_Id equals publish.Publisher_Id where booke.Name.Contains(BookName) select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = publish.Name, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            var bookrie = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id join publish in _context.Publisher on booke.Publisher.Publisher_Id equals publish.Publisher_Id where booke.Name.Contains(BookName) select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = publish.Name, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            return bookrie;
        }


        public void AddPublishers(BOBS_Backend.Models.Book.Publisher publishers)
        {
            _context.Publisher.Add(publishers);
            _context.SaveChanges();
        }

        public void AddGenres(BOBS_Backend.Models.Book.Genre genres)
        {
            _context.Genre.Add(genres);
            _context.SaveChanges();
        }

        public void AddBookTypes(BOBS_Backend.Models.Book.Type booktype)
        {
            _context.Type.Add(booktype);
            _context.SaveChanges();
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {

            var publishers = _context.Publisher;
            return publishers;
        }

        public IEnumerable<Genre> GetGenres()
        {

            var genres = _context.Genre;
            return genres;
        }
         
        public List<string> GetTypes()
        {
            List<string> typelist = new List<string>();
            var types = _context.Type;

            foreach( var i in types)
            {
                typelist.Add(i.TypeName);
            }

            return typelist; ;

        }
    }
}



