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
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace BOBS_Backend
{
    public class Inventory : IInventory
    {
        public DatabaseContext _context;
        private IHostingEnvironment _env;
        //   private readonly BOBS_Backend.ViewModel. _booksview;
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
            var books = _context.Book.Where(temp => temp.Name == book.Name && temp.Type == book.Type).ToList();
            if (books.Count == 0)
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
            var publishers = _context.Publisher.Find(publisher.Name);
            if (publishers == null)
            {
                _context.Publisher.Add(publisher);
                _context.SaveChanges();
            }
        }
        public IEnumerable<BookDetails> GetAllBooks()
        {
            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id select new BookDetails { Publisher = booke.Publisher, BookName = booke.Name, Price = price.ItemPrice, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
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

            var resizeStream = await ResizeImage(file, fileExt);

            if (_validImageExtensions.Contains(fileExt))
            {
                using (var fileStream = new FileStream(Path.Combine(dir, filename), FileMode.Create, FileAccess.Write))
                {
                    resizeStream.CopyTo(fileStream);
                }
                s3Client = new AmazonS3Client("AKIA6DYNIKQLFG4HU2LU", "4RM8WQL3tH4+c7RgIQ/LPBHqt6ESwleokqsDx1Gf", bucketRegion);


                var fileTransferUtility = new TransferUtility(s3Client);

                await fileTransferUtility.UploadAsync(Path.Combine(dir, filename), photosBucketName);

                url = String.Concat("https://ddkveyb1synl2.cloudfront.net/", file.FileName);
                return url;

            }
            return url;
        }

        public async Task<Stream> ResizeImage(IFormFile file, string fileExt)
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
                case "PNG":
                    encoder = new PngEncoder();
                    break;
                case "jpeg":
                case "JPEG":
                    encoder = new JpegEncoder();
                    break;
                case "jpg":
                case "JPG":
                    encoder = new JpegEncoder();
                    break;
                case "gif":
                case "GIF":
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

        public IEnumerable<BookDetails> GetRequestedBooks(string searchby, string searchfilter)
        {
            List<BookDetails> detail = new List<BookDetails>();
            if (searchby == "BookName")
            {

                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name.Contains(searchfilter) select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            if (searchby == "Publisher")
            {

                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Publisher.Name.Contains(searchfilter) select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            if (searchby == "Genre")
            {

                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Genre.Name.Contains(searchfilter) select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            if (searchby == "Condition")
            {

                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where price.Condition.ConditionName.Contains(searchfilter) select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            if (searchby == "Type")
            {

                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Type.TypeName.Contains(searchfilter) select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            return detail;
        }


        public int AddPublishers(BOBS_Backend.Models.Book.Publisher publishers)
        {

            var publishName = _context.Publisher.Where(publisher => publisher.Name == publishers.Name).ToList();

            if (publishName.Count == 0)
            {
                _context.Publisher.Add(publishers);
                _context.SaveChanges();
                return 0;
            }
            return 1;

        }

        public int AddGenres(BOBS_Backend.Models.Book.Genre genres)
        {

            var genreName = _context.Genre.Where(genre => genre.Name == genres.Name).ToList();

            if (genreName.Count == 0)
            {
                _context.Genre.Add(genres);
                _context.SaveChanges();
                return 0;
            }
            return 1;

        }

        //Saving an added book to the Inventory
        public int AddBookTypes(BOBS_Backend.Models.Book.Type booktype)
        {

            var typeName = _context.Type.Where(type => type.TypeName == booktype.TypeName).ToList();
            var typeId = _context.Type.Where(type => type.Type_Id == booktype.Type_Id).ToList();

            if (typeName.Count == 0)
            {
                _context.Type.Add(booktype);
                _context.SaveChanges();
                return 0;
            }
            return 1;

        }



        public int AddBookConditions(BOBS_Backend.Models.Book.Condition bookcondition)
        {
            _context.Condition.Add(bookcondition);
            _context.SaveChanges();

            var conditionName = _context.Condition.Where(condition => condition.ConditionName == bookcondition.ConditionName).ToList();

            if (conditionName.Count == 0)
            {
                _context.Condition.Add(bookcondition);
                _context.SaveChanges();
                return 0;
            }
            return 1;

        }

        //For Displaying All the existing Publishers in the drop down menu for AddInventory and Search Pages
        public List<BOBS_Backend.Models.Book.Publisher> GetAllPublishers()
        {

            var publishers = _context.Publisher.ToList();
            return publishers;
        }

        //For Displaying All the existing Genres in the drop down menu for AddInventory and Search Pages
        public List<BOBS_Backend.Models.Book.Genre> GetGenres()
        {

            var genres = _context.Genre.ToList();
            return genres;
        }

        //For Displaying All the existing Book Types in the drop down menu for AddInventory and Search Pages
        public List<BOBS_Backend.Models.Book.Type> GetTypes()
        {
            var typelist = _context.Type.ToList();
            return typelist;

        }

        //For Displaying All the existing BookConditions in the drop down menu for AddInventory and Search Pages
        public List<BOBS_Backend.Models.Book.Condition> GetConditions()
        {
            var conditions = _context.Condition.ToList();
            return conditions;

        }

        public BookDetails GetBookDetails(long bookid, long priceid)
        {

            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == bookid && price.Price_Id == priceid select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantiy, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();


            return booker[0];


        }

    }
}



