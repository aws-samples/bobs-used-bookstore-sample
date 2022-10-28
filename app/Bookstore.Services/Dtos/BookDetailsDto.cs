using System;
using Bookstore.Domain.Books;
using BookType = Bookstore.Domain.Books.BookType;

namespace DataAccess.Dtos
{
    public class BookDetailsDto
    {
        public string BookName { get; set; }

        public long BookId { get; set; }

        public Publisher Publisher { get; set; }

        public Condition BookCondition { get; set; }

        public BookType BookType { get; set; }

        public decimal Price { get; set; }

        public Genre Genre { get; set; }

        public int Quantity { get; set; }

        public string FrontUrl { get; set; }

        public string BackUrl { get; set; }

        public string LeftUrl { get; set; }

        public string RightUrl { get; set; }

        public string BookNameSearchParameter { get; set; }

        public string FrontPhoto { get; set; }

        public string BackPhoto { get; set; }

        public string RearPhoto { get; set; }

        public string LeftSidePhoto { get; set; }

        public string RightSidePhoto { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Author { get; set; }

        public bool Active { get; set; }

        public string Summary { get; set; }

        public string ISBN { get; set; }
    }
}