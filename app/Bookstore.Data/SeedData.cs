using Bookstore.Domain.Books;
using Bookstore.Domain.ReferenceData;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferenceDataItem>().HasData(
                new ReferenceDataItem { Id = 1, DataType = ReferenceDataType.BookType, Text = "Hardcover" },
                new ReferenceDataItem { Id = 2, DataType = ReferenceDataType.BookType, Text = "Trade Paperback" },
                new ReferenceDataItem { Id = 3, DataType = ReferenceDataType.BookType, Text = "Mass Market Paperback" },

                new ReferenceDataItem { Id = 4, DataType = ReferenceDataType.Condition, Text = "New" },
                new ReferenceDataItem { Id = 5, DataType = ReferenceDataType.Condition, Text = "Like New" },
                new ReferenceDataItem { Id = 6, DataType = ReferenceDataType.Condition, Text = "Good" },
                new ReferenceDataItem { Id = 7, DataType = ReferenceDataType.Condition, Text = "Acceptable" },

                new ReferenceDataItem { Id = 8, DataType = ReferenceDataType.Genre, Text = "Biographies" },
                new ReferenceDataItem { Id = 9, DataType = ReferenceDataType.Genre, Text = "Children's Books" },
                new ReferenceDataItem { Id = 10, DataType = ReferenceDataType.Genre, Text = "History" },
                new ReferenceDataItem { Id = 11, DataType = ReferenceDataType.Genre, Text = "Literature & Fiction" },
                new ReferenceDataItem { Id = 12, DataType = ReferenceDataType.Genre, Text = "Mystery, Thriller & Suspense" },
                new ReferenceDataItem { Id = 13, DataType = ReferenceDataType.Genre, Text = "Science Fiction & Fantasy" },
                new ReferenceDataItem { Id = 14, DataType = ReferenceDataType.Genre, Text = "Travel" },

                new ReferenceDataItem { Id = 15, DataType = ReferenceDataType.Publisher, Text = "Arcadia Books" },
                new ReferenceDataItem { Id = 16, DataType = ReferenceDataType.Publisher, Text = "Astral Publishing" },
                new ReferenceDataItem { Id = 17, DataType = ReferenceDataType.Publisher, Text = "Moonlight Publishing" },
                new ReferenceDataItem { Id = 18, DataType = ReferenceDataType.Publisher, Text = "Dreamscape Press" },
                new ReferenceDataItem { Id = 19, DataType = ReferenceDataType.Publisher, Text = "Enchanted Library" },
                new ReferenceDataItem { Id = 20, DataType = ReferenceDataType.Publisher, Text = "Fantasia House" },
                new ReferenceDataItem { Id = 21, DataType = ReferenceDataType.Publisher, Text = "Horizon Books" },
                new ReferenceDataItem { Id = 22, DataType = ReferenceDataType.Publisher, Text = "Infinity Press" },
                new ReferenceDataItem { Id = 23, DataType = ReferenceDataType.Publisher, Text = "Paradigm Publishing" },
                new ReferenceDataItem { Id = 24, DataType = ReferenceDataType.Publisher, Text = "Aurora Publishing" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Author = "Marley Cobb", PublisherId = 15, ISBN = "6556784356", BookTypeId = 1, Name = "2020 : The Apocalypse", GenreId =13, ConditionId = 5, CoverImageUrl = "/images/seedimages/apocalypse.png", Price = 10.95M, Quantity = 25 },
                new Book { Id = 2, Author = "Merritt Chambers", PublisherId = 16, ISBN = "7665438976", BookTypeId = 1, Name = "Children Of Iron", GenreId = 11, ConditionId = 6, CoverImageUrl = "/images/seedimages/childrenofiron.png", Price = 13.95M, Quantity = 3 },
                new Book { Id = 3, Author = "Lee Schuman", PublisherId = 17, ISBN = "5442280765", BookTypeId = 1, Name = "Gold In The Dark", GenreId = 13, ConditionId = 5, CoverImageUrl = "/images/seedimages/goldinthedark.png", Price = 6.50M, Quantity = 10 },
                new Book { Id = 4, Author = "Vick Hines", PublisherId = 18, ISBN = "4556789542", BookTypeId = 2, Name = "League Of Smokes", GenreId = 11, ConditionId = 7, CoverImageUrl = "/images/seedimages/leagueofsmokes.png", Price = 3M, Quantity = 1 },
                new Book { Id = 5, Author = "Bailey Armstrong", PublisherId = 19, ISBN = "4563358087", BookTypeId = 2, Name = "Alone With The Stars", GenreId = 12, ConditionId = 5, CoverImageUrl = "/images/seedimages/alonewiththestars.png", Price = 15.95M, Quantity = 5 },
                new Book { Id = 6, Author = "Owen Kain", PublisherId = 20, ISBN = "2354435678", BookTypeId = 1, Name = "The Girl In The Polaroid", GenreId = 12, ConditionId = 6, CoverImageUrl = "/images/seedimages/girlinthepolaroid.png", Price = 8.25M, Quantity = 2 },
                new Book { Id = 7, Author = "Adrian Lawrence", PublisherId = 21, ISBN = "6554789632", BookTypeId = 2, Name = "Nana Lawrence 10001 Jokes", GenreId = 11, ConditionId = 5, CoverImageUrl = "/images/seedimages/nana.png", Price = 13.95M, Quantity = 7 },
                new Book { Id = 8, Author = "Harlow Nicholas", PublisherId = 22, ISBN = "4558786554", BookTypeId = 3, Name = "My Search For Meaning", GenreId = 8, ConditionId = 7, CoverImageUrl = "/images/seedimages/mysearchformeaning.png", Price = 5M, Quantity = 15 }
            );

            base.OnModelCreating(modelBuilder);
        }

    }
}
