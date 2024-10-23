using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Data;
using Bookstore.Domain.Books;
using Amazon.Rekognition.Model;
using Bookstore.Data.Repositories;
using Bookstore.Domain.Tests.Builders;

[CollectionDefinition(nameof(BookRepositoryTests), DisableParallelization = true)]
public class BookRepositoryTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<Bookstore.Data.ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new ApplicationDbContext(options,true);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }

    private List<Book> CreateSampleBooks()
    {
        var books = new List<Book>
        {
            new BookBuilder().Name("Test").Build(),
            new BookBuilder().Name("Test").Build(),
            new BookBuilder().Name("Dummy 3").Build(),
            new BookBuilder().Build(),
        };
        return books;
    }


    [Fact]
    public async Task ListAsync_SearchStringFiltersCorrectly()
    {
        // Arrange
        using var context = CreateContext();
        context.Books.AddRange(CreateSampleBooks());
        await context.SaveChangesAsync();

        var repository = new BookRepository(context);

        // Act
        var result = await repository.ListAsync("Test", "Name", 1, 10);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, item => Assert.Contains("Test", item.Name));
    }

}
