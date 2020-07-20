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
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.S3.Model.Internal.MarshallTransformations;
using BOBS_Backend.Models.Order;
using Amazon.Runtime.Internal.Transform;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

        public BookDetails GetBookByID(long BookId)
        {
            var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == BookId select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();

            return book[0];

        }


        public void SaveBook(Book book)
        {
         
            
                _context.Book.Add(book);
                _context.SaveChanges();
            

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


        // public IEnumerable<Book> GetAllBooks()
        public IEnumerable<BookDetails> GetAllBooks()
        {
            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id  select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition,  BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            // var booker = _context.Book.ToList();

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


                bool check = await IsImageSafe(photosBucketName, filename);

                if (check)
                {
                    if (await IsBook(photosBucketName, filename))
                    {
                        url = String.Concat("https://dtdt6j0vhq1rq.cloudfront.net/", file.FileName);
                        return url;

                    }
                    else
                    {
                        return $"NotABook";

                    }
                }

                else
                {
                    return $"PolicyViolation";
                }

            }
            return $"InvalidFileType";
        }

        public async Task<Stream> ResizeImage(IFormFile file, string fileExt)
        {
            // create new memory stream.
            Stream result = new MemoryStream();
            int new_size = 300;
            // create new image variable
            using var img = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
            // set height and width proportional
            var div = img.Width / new_size;
            var hgt = Convert.ToInt32(Math.Round((decimal)(img.Height / div)));

            // change size of image
            img.Mutate(x => x.Resize(200, 200));
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
            HashSet<string> validlabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "book", "paper", "magazine", "newspaper" ,"storybook" ,"textbook" , "novel"
        };
            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient("AKIA6DYNIKQLFG4HU2LU", "4RM8WQL3tH4+c7RgIQ/LPBHqt6ESwleokqsDx1Gf", bucketRegion);

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
                MaxLabels = 8,
                MinConfidence = 75F
            };

            int c = 0;
            DetectLabelsResponse detectLabelsResponse = await rekognitionClient.DetectLabelsAsync(detectlabelsRequest);
            foreach (Label label in detectLabelsResponse.Labels)
                if (validlabels.Contains(label.Name) && label.Confidence >= 90)
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
            var RekognitionClient = new AmazonRekognitionClient("AKIA6DYNIKQLFG4HU2LU", "4RM8WQL3tH4+c7RgIQ/LPBHqt6ESwleokqsDx1Gf", bucketRegion);
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
                return false;
            }

            else
            {
                return true;
            }
        }
        //IEnumerable<Book>
        public IEnumerable<BookDetails> GetRequestedBooks(string searchby, string searchfilter)
        {
            List<BookDetails> detail = new List<BookDetails>();
            // List<Book> detail = new List<Book>();

            if (searchby == null)
            {
                //var booker = _context.Book.Where(booke => booke.Name.Contains(searchfilter));
                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name.Contains(searchfilter) || booke.Publisher.Name.Contains(searchfilter) || booke.Genre.Name.Contains(searchfilter) || price.Condition.ConditionName.Contains(searchfilter) || booke.Type.TypeName.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            if (searchby == "BookName")
            {
                //var booker = _context.Book.Where(booke => booke.Name.Contains(searchfilter));
                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            if (searchby == "Publisher")
            {
                // var booker = _context.Book.Where(booke => booke.Publisher.Name.Contains(searchfilter));
                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Publisher.Name.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            if (searchby == "Genre")
            {
                //var booker = _context.Book.Where(booke => booke.Genre.Name.Contains(searchfilter));
                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Genre.Name.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            if (searchby == "Condition")
            {

                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where price.Condition.ConditionName.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return booker;
            }

            if (searchby == "Type")
            {
                // var booker = _context.Book.Where(booke => booke.Type.TypeName.Contains(searchfilter));
                var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Type.TypeName.Contains(searchfilter) select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
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

            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == bookid && price.Price_Id == priceid select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();


            return booker[0];


        }


        public int AddToTables(BooksViewModel bookview)
        {
            string front_url = "", back_url = "", left_url = "", right_url = "", AudioBookUrl = "";
            if (bookview.FrontPhoto != null)
            {
                front_url = UploadtoS3(bookview.FrontPhoto).Result;
            }

            if (bookview.BackPhoto != null)
            {
                back_url = UploadtoS3(bookview.BackPhoto).Result;
            }

            if (bookview.LeftSidePhoto != null)
            {
                left_url = UploadtoS3(bookview.LeftSidePhoto).Result;
            }

            if (bookview.RightSidePhoto != null)
            {
                right_url = UploadtoS3(bookview.RightSidePhoto).Result;
            }

            if (checkIfViolation(front_url) == true || checkIfViolation(back_url) == true || checkIfViolation(left_url) == true || checkIfViolation(right_url) == true)
            {
                return 0;
            }

            if (bookview.Summary != null)
            {
                AudioBookUrl = GenerateAudioSummary(bookview.BookName, bookview.Summary, "fr-CA", VoiceId.Emma);
            }

            Book book = new Book();
            Price price = new Price();

            var publisherdata = _context.Publisher.Where(publisher => publisher.Name == bookview.PublisherName).ToList();
            var genredata = _context.Genre.Where(genre => genre.Name == bookview.genre).ToList();
            var typedata = _context.Type.Where(type => type.TypeName == bookview.BookType).ToList();
            var conditiondata = _context.Condition.Where(condition => condition.ConditionName == bookview.BookCondition).ToList();

            book.Name = bookview.BookName;
            book.Type = typedata[0];
            book.Genre = genredata[0];
            book.ISBN = bookview.ISBN;
            book.Publisher = publisherdata[0];
            book.Front_Url = front_url;
            book.Back_Url = back_url;
            book.Left_Url = left_url;
            book.Right_Url = right_url;
            book.Summary = bookview.Summary;
            book.AudioBook_Url = AudioBookUrl;

            price.Condition = conditiondata[0];
            price.ItemPrice = bookview.price;
            price.Book = book;
            price.Quantity = bookview.quantity;

            var books = _context.Book.Where(temp => temp.Name == book.Name && temp.Type == book.Type && temp.Publisher == book.Publisher && temp.Genre == book.Genre).ToList();
            if (books.Count == 0)
            {
                SaveBook(book);
                SavePrice(price);
            }

            else
            {
                price.Book = books[0];
                var prices = _context.Price.Where(p => p.Condition == price.Condition && p.Book.Name == book.Name).ToList();
                if (prices.Count == 0)
                {

                    SavePrice(price);
                }

                else
                {
                    var output = _context.Price.Where(p => p.Condition == price.Condition && p.Book.Name == book.Name).ToList();
                    output[0].Quantity = bookview.quantity;
                    output[0].ItemPrice = bookview.price;
                    _context.SaveChanges();
                }

            }

            return 1;
        }


        public bool checkIfViolation(string input)
        {

            if (input == "NotABook" || input == "PolicyViolation" || input == "InvalidFileType")
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public string GenerateAudioSummary(string BookName, string Summary, string targetLanguageCode, VoiceId voice)
        {
            using (var client = new AmazonPollyClient("AKIA6DYNIKQLFG4HU2LU", "4RM8WQL3tH4+c7RgIQ/LPBHqt6ESwleokqsDx1Gf", bucketRegion))
            {
                var request = new Amazon.Polly.Model.SynthesizeSpeechRequest();
                request.LanguageCode = targetLanguageCode;
                request.Text = Summary;
                request.OutputFormat = OutputFormat.Mp3;
                request.VoiceId = voice;
                var response = client.SynthesizeSpeechAsync(request).GetAwaiter().GetResult();

                string outputFileName = $".\\-{targetLanguageCode}.mp3";
                FileStream output = File.Open(outputFileName, FileMode.Create);
                response.AudioStream.CopyTo(output);
                output.Close();
                s3Client = new AmazonS3Client("AKIA6DYNIKQLFG4HU2LU", "4RM8WQL3tH4+c7RgIQ/LPBHqt6ESwleokqsDx1Gf", bucketRegion);
                var fileTransferUtility = new TransferUtility(s3Client);
                fileTransferUtility.UploadAsync(outputFileName, audioBucketName);

                var url = String.Concat("https://d3iukz826t8vlr.cloudfront.net/", BookName);
                return url;
            }
        }


        public IEnumerable<BookDetails> GetDetails(long BookId)
        {
            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == BookId select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            return booker;
        }

        public IEnumerable<BookDetails> SearchBeta(string searchby, string searchfilter)
        {
            List<long> list = new List<long>();
            List<string> names = new List<string>();
            List<BookDetails> books = new List<BookDetails>();
            List<BookDetails> booker = new List<BookDetails>();

            if (searchby == "BookName")
            {
                 booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            }

            if (searchby == "Publisher")
            {
                 booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Publisher.Name.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            }

            if (searchby == "Genre")
            {
                booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Genre.Name.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            }

            if (searchby == "Condition")
            {
                booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where price.Condition.ConditionName.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            }

            if (searchby == "Type")
            {
                booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Type.TypeName.Contains(searchfilter) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            }

            if (searchby == null)
            {
                booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name.Contains(searchfilter) || booke.Publisher.Name.Contains(searchfilter) || booke.Genre.Name.Contains(searchfilter) || price.Condition.ConditionName.Contains(searchfilter) || booke.Type.TypeName.Contains(searchfilter)  select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            }

            foreach (var i in booker)
                {
                    if (!names.Contains(i.BookName))
                    {
                        list.Add(i.BookId);
                        names.Add(i.BookName);
                   
                    }
                }

                foreach (long i in list)
                {

                    var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == i select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                    books.Add(book[0]);
                }

            return books;

        }


        public List<string> GetTypesOfheBook(string bookname)
        {
            List<string> types = new List<string>();
            var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name ==  bookname select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();

            foreach (var i in book)
            {
                if (!types.Contains(i.BookType.TypeName))
                {
                    types.Add(i.BookType.TypeName);

                }
            }

            return types;
        }

        public List<BookDetails> GetRelevantBooks(string Bookname , string type)
        {
            var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == Bookname && booke.Type.TypeName == type select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();

            return book;
        }

        public List<Dictionary<string,int>> DashBoard()
        {

            var list =  _context.OrderDetail                                   
                                    .Include(o => o.Book)
                                        .ThenInclude(b => b.Genre)
                                    .Include(o => o.Book)
                                        .ThenInclude(b => b.Publisher)
                                    .Include(o => o.Book)
                                        .ThenInclude(b => b.Type)
                                    .Include(o => o.Price)
                                        .ThenInclude(p => p.Condition)
                                    .ToList();

            var delivered = _context.Order.Where(x => x.OrderStatus.Status == "Delivered").ToList().Count();
            var justplaced = _context.Order.Where(x => x.OrderStatus.Status == "Just Placed").ToList().Count();
            var enroute = _context.Order.Where(x => x.OrderStatus.Status == "En Route").ToList().Count();
            var pending = _context.Order.Where(x => x.OrderStatus.Status == "Pending").ToList().Count();
            var ordersplaced = delivered + justplaced + enroute + pending;

            Dictionary<string, int> genre_stats = new Dictionary<string, int>();
            Dictionary<string, int> publisher_stats = new Dictionary<string, int>();
            Dictionary<string, int> condition_stats = new Dictionary<string, int>();
            Dictionary<string, int> bookname_stats = new Dictionary<string, int>();
            Dictionary<string, int> type_stats = new Dictionary<string, int>();
            Dictionary<string, int> count_stats = new Dictionary<string, int>();

            int total_quantity = 0;
            double total_sales = 0;
            int total_quantity_sold = 0;
            int total_merchandise = 0;
            int total_books = 0;
            int total_titles = 0;

            var lis = _context.OrderDetail.ToList();
            foreach (var i in lis)
            {
                total_quantity_sold = total_quantity_sold + i.quantity;
                total_sales = total_sales + i.price*i.quantity;

            }

            var books = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            foreach(var i in books)
            {
                total_books = total_books + i.Quantity;
                total_merchandise = total_merchandise + (int)i.Price*i.Quantity;
            }

            total_titles = _context.Book.ToList().Count();




            foreach ( var detail in list)
            {
                //Genre
                if (!genre_stats.ContainsKey(detail.Book.Genre.Name))
                {
                    genre_stats.Add(detail.Book.Genre.Name, 1);
                }

                else
                {

                    var count = genre_stats.GetValueOrDefault(detail.Book.Genre.Name);
                    genre_stats[detail.Book.Genre.Name] =  count + 1;

                }

                //Publisher
                if (!publisher_stats.ContainsKey(detail.Book.Publisher.Name))
                {
                    publisher_stats.Add(detail.Book.Publisher.Name, 1);
                }

                else
                {

                    var count = publisher_stats.GetValueOrDefault(detail.Book.Publisher.Name);
                    publisher_stats[detail.Book.Publisher.Name] = count + 1;

                }
              
                //BookName
                if (!bookname_stats.ContainsKey(detail.Book.Name))
                {
                    bookname_stats.Add(detail.Book.Name, 1);
                }

                else
                {

                    var count = bookname_stats.GetValueOrDefault(detail.Book.Name);
                    bookname_stats[detail.Book.Name] = count + 1;

                }

                //Type
                if (!type_stats.ContainsKey(detail.Book.Type.TypeName))
                {
                    type_stats.Add(detail.Book.Type.TypeName, 1);
                }

                else
                {

                    var count = type_stats.GetValueOrDefault(detail.Book.Type.TypeName);
                    type_stats[detail.Book.Type.TypeName] = count + 1;

                }

            }

            count_stats["total_books_sold"] = total_quantity_sold;
            count_stats["total_sales"] = (int)total_sales;
            count_stats["total_books"] = total_books;
            count_stats["total_titles"] = total_titles;
            count_stats["total_merchandise_value"] = total_merchandise;       
            count_stats["total_genres"] = GetGenres().Count();
            count_stats["total_publishers"] = GetAllPublishers().Count();
            count_stats["total_types"] = GetTypes().Count();
            count_stats["total_conditions"] = GetConditions().Count();
            count_stats["Orders_placed"] = ordersplaced;
            count_stats["delivered"] = delivered;
            count_stats["enroute"] = enroute;
            count_stats["pending"] = pending;
            count_stats["justplaced"] = justplaced;
 
            List<Dictionary<string, int>> stats = new List<Dictionary<string, int>>();
            genre_stats = (from entry in genre_stats orderby entry.Value descending select entry)
                    .Take(5)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

            type_stats = (from entry in type_stats orderby entry.Value descending select entry)
                    .Take(5)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

            publisher_stats = (from entry in publisher_stats orderby entry.Value descending select entry)
                    .Take(5)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

            bookname_stats = (from entry in bookname_stats orderby entry.Value descending select entry)
                    .Take(5)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

            stats.Add(genre_stats);
            stats.Add(type_stats);
            stats.Add(publisher_stats);
            stats.Add(bookname_stats);
            stats.Add(count_stats);

            return stats;
        }
        
        public BookDetails UpdateDetails(int Id , string Condition)
        {

            var list = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == Id && price.Condition.ConditionName == Condition select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();

            return list[0];
        }

        public void PushDetails(BookDetails details)
        {
              var output = _context.Price.Where(p => p.Condition.ConditionName == details.BookCondition.ConditionName && p.Book.Book_Id == details.BookId).ToList();
            if (details.FrontPhoto != null)
            {
                details.front_url = UploadtoS3(details.FrontPhoto).Result;
            }
           
            if (details.BackPhoto != null)
            {
                details.back_url = UploadtoS3(details.BackPhoto).Result;
            }

        
            if (details.LeftSidePhoto != null)
            {
                details.left_url = UploadtoS3(details.LeftSidePhoto).Result;
            }
        
            if (details.RightSidePhoto != null)
            {
                details.right_url = UploadtoS3(details.RightSidePhoto).Result;
            }

            var book = _context.Book.Where(p => p.Book_Id == details.BookId).ToList();
            book[0].Front_Url = details.front_url;
            book[0].Back_Url = details.back_url;
            book[0].Left_Url = details.left_url;
            book[0].Right_Url = details.right_url;
            output[0].Quantity = details.Quantity;
            output[0].ItemPrice = details.Price;
            _context.SaveChanges();
        }

    }


}



