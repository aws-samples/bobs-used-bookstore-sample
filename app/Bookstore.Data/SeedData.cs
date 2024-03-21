using Bookstore.Domain.Books;
using Bookstore.Domain.ReferenceData;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        private void PopulateDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferenceDataItem>().HasData(
                new ReferenceDataItem(ReferenceDataType.BookType, "Hardcover") { Id = 1 },
                new ReferenceDataItem(ReferenceDataType.BookType, "Trade Paperback") { Id = 2 },
                new ReferenceDataItem(ReferenceDataType.BookType, "Mass Market Paperback") { Id = 3 },

                new ReferenceDataItem(ReferenceDataType.Condition, "New") { Id = 4 },
                new ReferenceDataItem(ReferenceDataType.Condition, "Like New") { Id = 5 },
                new ReferenceDataItem(ReferenceDataType.Condition, "Good") { Id = 6 },
                new ReferenceDataItem(ReferenceDataType.Condition, "Acceptable") { Id = 7 },

                new ReferenceDataItem(ReferenceDataType.Genre, "Biographies") { Id = 8 },
                new ReferenceDataItem(ReferenceDataType.Genre, "Children's Books") { Id = 9 },
                new ReferenceDataItem(ReferenceDataType.Genre, "History") { Id = 10 },
                new ReferenceDataItem(ReferenceDataType.Genre, "Literature & Fiction") { Id = 11 },
                new ReferenceDataItem(ReferenceDataType.Genre, "Mystery, Thriller & Suspense") { Id = 12 },
                new ReferenceDataItem(ReferenceDataType.Genre, "Science Fiction & Fantasy") { Id = 13 },
                new ReferenceDataItem(ReferenceDataType.Genre, "Travel") { Id = 14 },

                new ReferenceDataItem(ReferenceDataType.Publisher, "Arcadia Books") { Id = 15 },
                new ReferenceDataItem(ReferenceDataType.Publisher, "Astral Publishing") { Id = 16 },
                new ReferenceDataItem(ReferenceDataType.Publisher, "Moonlight Publishing") { Id = 17 },
                new ReferenceDataItem(ReferenceDataType.Publisher, "Dreamscape Press") { Id = 18 },
                new ReferenceDataItem(ReferenceDataType.Publisher, "Enchanted Library") { Id = 19 },
                new ReferenceDataItem(ReferenceDataType.Publisher, "Fantasia House") { Id = 20 },
                new ReferenceDataItem(ReferenceDataType.Publisher, "Horizon Books") { Id = 21 },
                new ReferenceDataItem(ReferenceDataType.Publisher, "Infinity Press") { Id = 22 },
                new ReferenceDataItem(ReferenceDataType.Publisher, "Paradigm Publishing") { Id = 23 },
                new ReferenceDataItem(ReferenceDataType.Publisher, "Aurora Publishing") { Id = 24 }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book("2020: The Apocalypse", "Li Juan", "6556784356", 15, 1, 13, 5, 10.95M, 25, null, null, "/images/coverimages/apocalypse.png") { Id = 1 },
                new Book("Children Of Iron", "Nikki Wolf", "7665438976", 16, 1, 11, 6, 13.95M, 3, null, null, "/images/coverimages/childrenofiron.png") { Id = 2 },
                new Book("Gold In The Dark", "Richard Roe", "5442280765", 17, 1, 13, 5, 6.50M, 10, null, null, "/images/coverimages/goldinthedark.png") { Id = 3 },
                new Book("Leagues Of Smoke", "Pat Candella", "4556789542", 18, 2, 11, 7, 3M, 1, null, null, "/images/coverimages/leaguesofsmoke.png") { Id = 4 },
                new Book("Alone With The Stars", "Carlos Salazar", "4563358087", 19, 2, 12, 5, 15.95M, 5, null, null, "/images/coverimages/alonewiththestars.png") { Id = 5 },
                new Book("The Girl In The Polaroid", "Terri Whitlock", "2354435678", 20, 1, 12, 6, 8.25M, 2, null, null, "/images/coverimages/girlinthepolaroid.png") { Id = 6 },
                new Book("1001 Jokes", "Mary Major", "6554789632", 21, 2, 11, 5, 13.95M, 7, null, null, "/images/coverimages/1001jokes.png") { Id = 7 },
                new Book("My Search For Meaning", "Mateo Jackson", "4558786554", 22, 3, 8, 7, 5M, 15, null, null, "/images/coverimages/mysearchformeaning.png") { Id = 8 }
            );
        }
    }
}