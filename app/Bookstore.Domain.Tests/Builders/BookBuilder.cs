using Bookstore.Domain.Books;

namespace Bookstore.Domain.Tests.Builders;

public class BookBuilder
{
    private string name = "test";
    private string author = "author";
    private string isbn = "12345678";
    private int publisherId = 1;
    private int bookTypeId = 2;
    private int genreId = 3;
    private int conditionId = 4;
    private decimal price = 5;
    private int quantity = 6;
    private int? year = 2000;
    private string? summary = "book summary";
    private string? coverImageUrl = "http://cover.dummy";
    
    public Book Build()
    {
        return new Book(name, author, isbn, publisherId, bookTypeId, genreId, conditionId, price, quantity, year,
            summary, coverImageUrl);
    }
    
    public BookBuilder Name(string value)
    {
        name = value;
        return this;
    }
    
    public BookBuilder Author(string value)
    {
        author = value;
        return this;
    }   
    
    public BookBuilder ISBN(string value)
    {
        isbn = value;
        return this;
    }    
    
    public BookBuilder PublisherId(int value)
    {
        publisherId = value;
        return this;
    } 
    
    public BookBuilder BookTypeId(int value)
    {
        bookTypeId = value;
        return this;
    } 
    
    public BookBuilder GenreId(int value)
    {
        genreId = value;
        return this;
    } 
    
    public BookBuilder ConditionId(int value)
    {
        conditionId = value;
        return this;
    }  
    
    public BookBuilder Price(decimal value)
    {
        price = value;
        return this;
    }
    
    public BookBuilder Quantity(int value)
    {
        quantity = value;
        return this;
    } 
    
    public BookBuilder Year(int? value)
    {
        year = value;
        return this;
    }    
    
    public BookBuilder Summary(string? value)
    {
        summary = value;
        return this;
    }   
    
    public BookBuilder CoverImageUrl(string? value)
    {
        coverImageUrl = value;
        return this;
    } 
}