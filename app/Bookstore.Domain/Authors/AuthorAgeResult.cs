namespace Bookstore.Domain.Authors;

public class AuthorAgeResult
{
    public int BusinessEntityID { get; set; }
    public string FormattedModifiedDate { get; set; } = string.Empty;
    public int Age { get; set; }
}