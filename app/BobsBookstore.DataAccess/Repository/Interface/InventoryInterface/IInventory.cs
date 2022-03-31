using System.Collections.Generic;
using BobsBookstore.Models.Books;
using BobsBookstore.DataAccess.Dtos;

namespace BobsBookstore.DataAccess.Repository.Interface.InventoryInterface
{
    public interface IInventory
    {
        public SearchBookDto SearchBooks(string searchBy, string searchFilter, string style, string sortBy, int pageNum, string ascDesc);

        public void SaveBook(Book book );

        public SearchBookDto GetAllBooks(int pageNum, string style, string sortBy, string ascDesc);

        public BookDetailsDto GetBookByID(long bookId);

        public void SavePrice(Price price);
      
        public bool AddPublishers(Publisher publishers);

        public bool AddGenres(Genre genres);

        public bool AddBookTypes(Type bookType);

        public bool AddBookConditions(Condition bookCondition);

        public List<Type> GetTypes();

        public List<Publisher> GetAllPublishers();

        public List<Genre> GetGenres();

        public List<Condition> GetConditions();

        public BookDetailsDto GetBookDetails(long bookId, long priceId);

        public bool AddToTables(BooksDto booksDto);

        public IEnumerable<BookDetailsDto> GetDetails(long bookId);

        public List<string> GetFormatsOfTheSelectedBook(string bookName);

        public List<string> GetConditionsOfTheSelectedBook(string bookName);

        public List<BookDetailsDto> GetRelevantBooks(string bookName, string type, string conditionChosen);

        public List<Dictionary<string, int>> DashBoard(int numberOfDetails);

        public void PushDetails(BookDetailsDto bookdetailsDto);

        public List<string> autosuggest(string input);

        public BookDetailsDto UpdateDetails(int id, string condition);

        public void EditPublisher(string oldPublisherName, string newPublisherName);

        public void EditGenre(string oldGenre, string newGenre);

        public void EditCondition(string oldCondition, string newCondition);

        public void EditType(string oldType, string newType);

        public List<string> ScreenInventory();
    }
}