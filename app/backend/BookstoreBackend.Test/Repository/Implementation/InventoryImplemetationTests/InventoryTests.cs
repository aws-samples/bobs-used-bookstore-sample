using System;
using Xunit;
using Bobs_Backend;
using BOBS_Backend.Repository.Implementations.WelcomePageImplementation;
using Moq;
using Microsoft.EntityFrameworkCore.Storage;
using BOBS_Backend.Database;
using System.Threading.Tasks;
using BOBS_Backend.Models.Book;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Autofac.Extras.Moq;
using Bobs_Backend.Test.Repository;
using BOBS_Backend;
using BOBS_Backend.Repository.Implementations;
using Amazon.Runtime.Internal.Util;
using BOBS_Backend.Repository.SearchImplementations;
using Microsoft.Extensions.Logging;

namespace Bobs_Backend.Test
{
    public class InventoryTests
    {
        private readonly Inventory _sut;
        private readonly Mock<DatabaseContext> _mockDatabaseContext = new Mock<DatabaseContext>();
        private readonly Mock<IInventory> _mockInventoryRepo = new Mock<IInventory>();
        private readonly Mock<IRekognitionNPollyRepository> _mockRekognitionRepo = new Mock<IRekognitionNPollyRepository>();
        private readonly Mock<Microsoft.Extensions.Logging.ILogger<Inventory>> _mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<Inventory>>();
        private readonly Mock<ISearchRepository> _mockSearch = new Mock<ISearchRepository>();

        [Fact]
        public void AutoSugggestionss_ShouldReturnNULL_WhenDBTableIsEmpty()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();          
            var Input = "f";
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var Suggestions = _sut.autosuggest(Input);
            Assert.Empty(Suggestions);
            context.Dispose();
        }

        [Fact]
        public void AutoSugggestionss_ShouldReturnNULL_WhenNoRelevantResultsFound()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            context.Genre.Add(
                    new Genre
                    {
                        Genre_Id = 10,
                        Name = "Fantasy",
                        RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            context.SaveChanges();
            var Input = "ha";


            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var Suggestions = _sut.autosuggest(Input);
            Assert.Empty(Suggestions);
            context.Dispose();
        }

        [Fact]
        public void AutoSugggestionss_ShouldReturnSuggestions_WhenRelevantResultsFound()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            context.Genre.Add(
                    new Genre
                    {
                        Genre_Id = 10,
                        Name = "Fantasy",
                        RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            context.SaveChanges();
            var Input = "f";

            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var Suggestions = _sut.autosuggest(Input);
            Assert.NotNull(Suggestions);
            context.Dispose();
        }

        [Fact]
        public void GetBookById_ShouldReturnBook_WhenRelevantResultsFound()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            context.Price.Add(
                    new Price
                    {
                        Price_Id = 29,
                        Book = new Book()
                        {
                            Name = "ChichaTales",
                            ISBN = 12131231231,
                            Author = "Rahul",
                            RowVersion = new byte[] { 0x20, 0x20 },
                            Book_Id = 12,
                            Front_Url = "",
                            Back_Url = "",
                            Left_Url = "",
                            Right_Url = "",
                            Summary = "",
                            AudioBook_Url = "",
                            Publisher = new Publisher()
                            {
                                Publisher_Id = 10,
                                Name = "Greygoose",
                                RowVersion = new byte[] { 0x20, 0x20 }

                            },

                            Type = new BOBS_Backend.Models.Book.Type()
                            {
                                Type_Id = 12,
                                TypeName = "HardCover",
                                RowVersion = new byte[] { 0x20, 0x20 }
                            },

                            Genre = new Genre()
                            {
                                Genre_Id = 5,
                                Name = "Fantasy",
                                RowVersion = new byte[] { 0x20, 0x20 }

                            }
                        },
                        Condition = new Condition(),
                        ItemPrice = 154,
                        Quantity = 30,
                        UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                        Active = false,
                        RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );

            context.SaveChanges();
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            long BookID = 12;
            var Book = _sut.GetBookByID(BookID);
            Assert.NotNull(Book);
            context.Dispose();
        }

        [Fact]
        public void GetBookById_ShouldReturnNull_WhenNoRelevantResultsFound()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            context.Price.Add(
                   new Price
                   {
                       Price_Id = 29,
                       Book = new Book()
                       {
                           Name = "ChichaTales",
                           ISBN = 12131231231,
                           Author = "Rahul",
                           RowVersion = new byte[] { 0x20, 0x20 },
                           Book_Id = 12,
                           Front_Url = "",
                           Back_Url = "",
                           Left_Url = "",
                           Right_Url = "",
                           Summary = "",
                           AudioBook_Url = "",
                           Publisher = new Publisher()
                           {
                               Publisher_Id = 10,
                               Name = "Greygoose",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           },

                           Type = new BOBS_Backend.Models.Book.Type()
                           {
                               Type_Id = 12,
                               TypeName = "HardCover",
                               RowVersion = new byte[] { 0x20, 0x20 }
                           },

                           Genre = new Genre()
                           {
                               Genre_Id = 5,
                               Name = "Fantasy",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           }
                       },
                       Condition = new Condition(),
                       ItemPrice = 154,
                       Quantity = 30,
                       UpdatedBy = "admin",
                       UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                       Active = false,
                       RowVersion = new byte[] { 0x20, 0x20 }
                   }
               );
            context.SaveChanges();
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            long BookID = 10;
            var Book = _sut.GetBookByID(BookID);
            Assert.Null(Book);
            context.Dispose();
        }

        [Fact]
        public void GetBookById_ShouldReturnNull_WhenTableIsEmpty()
        {

            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            context.Book.Add(
                   new Book()
                   {
                       
                   }
                );
            context.SaveChanges();
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            long BookID = 10;
            var Book = _sut.GetBookByID(BookID);
            Assert.Null(Book);
            context.Dispose();
        }

        [Fact]
        public void EditPublishers_ShouldReturnTrue_WhenSuccess()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            context.Publisher.Add(new Publisher()
            {
                Publisher_Id = 10,
                Name = "Greygoose",
                RowVersion = new byte[] { 0x20, 0x20 }

            });
            context.SaveChanges();
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            string Actual = "Greygoose";
            string New = "Grey";
            _sut.EditPublisher(Actual,New);          
        }

        [Fact]
        public void GetAllPublishers_ShouldReturnFalse_WhenTableIsEmpty()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var publishers = _sut.GetAllPublishers();
            Assert.Empty(publishers);
            context.Dispose();
        }

        [Fact]
        public void GetAllPublishers_ShouldReturnTrue_WhenTableIsNotEmpty()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            context.Publisher.Add(new Publisher()
            {
                Publisher_Id = 10,
                Name = "Greygoose",
                RowVersion = new byte[] { 0x20, 0x20 }

            });

            context.Publisher.Add(new Publisher()
            {
                Publisher_Id = 11,
                Name = "Ciroc",
                RowVersion = new byte[] { 0x20, 0x20 }

            });
            context.SaveChanges();
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var publishers = _sut.GetAllPublishers();
            Assert.NotEmpty(publishers);
            context.Dispose();
        }

        [Fact]
        public void AddToTables_ShouldReturnTrue_WhenNoImageButOtherTablesExist()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            BOBS_Backend.ViewModel.BooksViewModel booksView = new BOBS_Backend.ViewModel.BooksViewModel()
            {
                Active = true,
                Name = "LeoTolstoy",
                Summary = "",
                FrontPhoto = null,
                BackPhoto = null,
                LeftSidePhoto = null,
                RightSidePhoto = null,
                ISBN = 122312312,
                AudioBookUrl = "",
                quantity = 12,
                price = 20,
                Author = "Raul",
                genre = "Fantasy",
                PublisherName = "Greygoose",
                BookType = "HardCover",
                BookCondition = "Old",

            };
            context.Publisher.Add(
                new Publisher()
                {
                    Publisher_Id = 10,
                    Name = "Greygoose",
                    RowVersion = new byte[] { 0x20, 0x20 }

                });

            context.Type.Add(
                new BOBS_Backend.Models.Book.Type()
                {
                    Type_Id = 12,
                    TypeName = "HardCover",
                    RowVersion = new byte[] { 0x20, 0x20 }
                });

            context.Genre.Add(
                new Genre()
                {
                    Genre_Id = 5,
                    Name = "Fantasy",
                    RowVersion = new byte[] { 0x20, 0x20 }

                });

            context.Condition.Add(
                new Condition()
                {
                    Condition_Id = 12,
                    ConditionName = "Old",
                });
            context.SaveChanges();

            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var status = _sut.AddToTables(booksView);
            Assert.Equal(1,status);
            context.Dispose();
        }

        [Fact]
        public void AddToTables_ShouldReturnTrue_WhenNoImageButAtleastOneOfOtherTableIsEmpty()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            BOBS_Backend.ViewModel.BooksViewModel booksView = new BOBS_Backend.ViewModel.BooksViewModel()
            {
                Active = true,
                Name = "LeoTolstoy",
                Summary = "",
                FrontPhoto = null,
                BackPhoto = null,
                LeftSidePhoto = null,
                RightSidePhoto = null,
                ISBN = 122312312,
                AudioBookUrl = "",
                quantity = 12,
                price = 20,
                Author = "Raul",
                genre = "Fantasy",
                PublisherName = "Greygoose",
                BookType = "HardCover",
                BookCondition = "Old",

            };
            context.Publisher.Add(
                new Publisher()
                {
                    Publisher_Id = 10,
                    Name = "Greygoose",
                    RowVersion = new byte[] { 0x20, 0x20 }

                });

            context.Type.Add(
                new BOBS_Backend.Models.Book.Type()
                {
                    Type_Id = 12,
                    TypeName = "HardCover",
                    RowVersion = new byte[] { 0x20, 0x20 }
                });

            context.Genre.Add(
                new Genre()
                {
                    Genre_Id = 5,
                    Name = "Fantasy",
                    RowVersion = new byte[] { 0x20, 0x20 }

                });

            context.Condition.Add(
                new Condition()
                {
                   
                });
            context.SaveChanges();

            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var status = _sut.AddToTables(booksView);
            Assert.Equal(1,status);
            context.Dispose();
        }

        [Fact]
        public void GetFormatsOfTheSelectedBook_ShouldReturnTrue_WhenListisNotEmpty()
        {

            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            context.Price.Add(
                   new Price
                   {
                       Price_Id = 29,
                       Book = new Book()
                       {
                           Name = "ChichaTales",
                           ISBN = 12131231231,
                           Author = "Rahul",
                           RowVersion = new byte[] { 0x20, 0x20 },
                           Book_Id = 12,
                           Front_Url = "",
                           Back_Url = "",
                           Left_Url = "",
                           Right_Url = "",
                           Summary = "",
                           AudioBook_Url = "",
                           Publisher = new Publisher()
                           {
                               Publisher_Id = 10,
                               Name = "Greygoose",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           },

                           Type = new BOBS_Backend.Models.Book.Type()
                           {
                               Type_Id = 12,
                               TypeName = "HardCover",
                               RowVersion = new byte[] { 0x20, 0x20 }
                           },

                           Genre = new Genre()
                           {
                               Genre_Id = 5,
                               Name = "Fantasy",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           }
                       },
                       Condition = new Condition(),
                       ItemPrice = 154,
                       Quantity = 30,
                       UpdatedBy = "admin",
                       UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                       Active = false,
                       RowVersion = new byte[] { 0x20, 0x20 }
                   }
               );
            context.Price.Add(
                   new Price
                   {
                       Price_Id = 39,
                       Book = new Book()
                       {
                           Name = "ChichaTales",
                           ISBN = 12131231231,
                           Author = "Rahul",
                           RowVersion = new byte[] { 0x20, 0x20 },
                           Book_Id = 13,
                           Front_Url = "",
                           Back_Url = "",
                           Left_Url = "",
                           Right_Url = "",
                           Summary = "",
                           AudioBook_Url = "",
                           Publisher = new Publisher()
                           {
                               Publisher_Id = 10,
                               Name = "Greygoose",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           },

                           Type = new BOBS_Backend.Models.Book.Type()
                           {
                               Type_Id = 12,
                               TypeName = "SoftCover",
                               RowVersion = new byte[] { 0x20, 0x20 }
                           },

                           Genre = new Genre()
                           {
                               Genre_Id = 5,
                               Name = "Fantasy",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           }
                       },
                       Condition = new Condition(),
                       ItemPrice = 154,
                       Quantity = 30,
                       UpdatedBy = "admin",
                       UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                       Active = false,
                       RowVersion = new byte[] { 0x20, 0x20 }
                   }
               );
            context.SaveChanges();
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            string BookName = "Harry";
            var formats = _sut.GetFormatsOfTheSelectedBook(BookName);         
            Assert.NotEmpty(formats);
            context.Dispose();
        }

        [Fact]
        public void ScreenInventory_ShouldReturnTrue_WhenBooksTableIsEmpty()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();           
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var inventory = _sut.ScreenInventory();
            Assert.Empty(inventory);
            context.Dispose();
        }

        [Fact]
        public void ScreenInventory_ShouldReturnTrue_WhenBooksTableIsNotEmptyButNoBookRunningLow()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            context.Price.Add(
                   new Price
                   {
                       Price_Id = 39,
                       Book = new Book()
                       {
                           Name = "ChichaTales",
                           ISBN = 12131231231,
                           Author = "Rahul",
                           RowVersion = new byte[] { 0x20, 0x20 },
                           Book_Id = 13,
                           Front_Url = "",
                           Back_Url = "",
                           Left_Url = "",
                           Right_Url = "",
                           Summary = "",
                           AudioBook_Url = "",
                           Publisher = new Publisher()
                           {
                               Publisher_Id = 10,
                               Name = "Greygoose",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           },

                           Type = new BOBS_Backend.Models.Book.Type()
                           {
                               Type_Id = 12,
                               TypeName = "SoftCover",
                               RowVersion = new byte[] { 0x20, 0x20 }
                           },

                           Genre = new Genre()
                           {
                               Genre_Id = 5,
                               Name = "Fantasy",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           }
                       },
                       Condition = new Condition(),
                       ItemPrice = 154,
                       Quantity = 30,
                       UpdatedBy = "admin",
                       UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                       Active = false,
                       RowVersion = new byte[] { 0x20, 0x20 }
                   }
               );
            context.SaveChanges();
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var inventory = _sut.ScreenInventory();
            Assert.Empty(inventory);
            context.Dispose();
        }

        [Fact]
        public void ScreenInventory_ShouldReturnTrue_WhenBooksTableIsNotEmptyButAtleastOneBookRunningLow()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            context.Price.Add(
                   new Price
                   {
                       Price_Id = 39,
                       Book = new Book()
                       {
                           Name = "ChichaTales",
                           ISBN = 12131231231,
                           Author = "Rahul",
                           RowVersion = new byte[] { 0x20, 0x20 },
                           Book_Id = 13,
                           Front_Url = "",
                           Back_Url = "",
                           Left_Url = "",
                           Right_Url = "",
                           Summary = "",
                           AudioBook_Url = "",
                           Publisher = new Publisher()
                           {
                               Publisher_Id = 10,
                               Name = "Greygoose",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           },

                           Type = new BOBS_Backend.Models.Book.Type()
                           {
                               Type_Id = 12,
                               TypeName = "SoftCover",
                               RowVersion = new byte[] { 0x20, 0x20 }
                           },

                           Genre = new Genre()
                           {
                               Genre_Id = 5,
                               Name = "Fantasy",
                               RowVersion = new byte[] { 0x20, 0x20 }

                           }
                       },
                       Condition = new Condition(),
                       ItemPrice = 154,
                       Quantity = 2,
                       UpdatedBy = "admin",
                       UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                       Active = false,
                       RowVersion = new byte[] { 0x20, 0x20 }
                   }
               );
            context.SaveChanges();
            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var inventory = _sut.ScreenInventory();
            Assert.NotEmpty(inventory);
            context.Dispose();
        }   
        


    }
}
