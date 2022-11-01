namespace Bookstore.Domain.Books
{
    public class Book : Entity
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public Publisher Publisher { get; set; }

        public string ISBN { get; set; }

        public BookType Type { get; set; }

        public Genre Genre { get; set; }

        //public Condition Condition { get; set; }

        public string FrontUrl { get; set; }

        public string BackUrl { get; set; }

        public string LeftUrl { get; set; }

        public string RightUrl { get; set; }

        public string AudioBookUrl { get; set; }

        public string Summary { get; set; }
    }
}