using System.Collections.Generic;
using BobsBookstore.Models.Books;
using BobsBookstore.DataAccess.Dtos;

namespace BobsBookstore.DataAccess.Repository.Interface.InventoryInterface
{
    public interface IInventory
    {
        public SearchBookDto SearchBooks(string searchby, string searchfilter, string style, string SortBy, int pagenum, string ascdesc);

        public void SaveBook(Book book );

        public SearchBookDto GetAllBooks(int pagenum, string style, string SortBy, string ascdesc);

        public BookDetailsDto GetBookByID(long Id);

        public void SavePrice(Price price);
      
        public int AddPublishers(Publisher publishers);

        public int AddGenres(Genre genres);

        public int AddBookTypes(Type booktype);

        public int AddBookConditions(Condition bookcondition);

        public List<Type> GetTypes();

        public List<Publisher> GetAllPublishers();

        public List<Genre> GetGenres();

        public List<Condition> GetConditions();

        public BookDetailsDto GetBookDetails(long bookid, long priceid);

        public int AddToTables(BooksDto bookview);

        public IEnumerable<BookDetailsDto> GetDetails(long BookId);


        public List<string> GetFormatsOfTheSelectedBook(string bookname);

        public List<string> GetConditionsOfTheSelectedBook(string bookname);

        public List<BookDetailsDto> GetRelevantBooks(string Bookname , string type , string condition_chosen);

        public List<Dictionary<string, int>> DashBoard();


        public void PushDetails(BookDetailsDto details);

        public List<string> autosuggest(string input);

        public BookDetailsDto UpdateDetails(int Id, string Condition);

        public void EditPublisher(string Actual, string New);

        public void EditGenre(string Actual, string New);

        public void EditCondition(string Actual, string New);

        public void EditType(string Actual, string New);

        public List<string> ScreenInventory();



    }
}