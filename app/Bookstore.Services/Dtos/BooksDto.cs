using System;
using System.Collections.Generic;

namespace DataAccess.Dtos
{
    public class BooksDto
    {
        public string BookName { get; set; }

        public string ISBN { get; set; }

        public long BookId { get; set; }

        public long PublisherId { get; set; }

        public string PublisherName { get; set; }

        public string BookType { get; set; }

        public string Genre { get; set; }

        public string BookCondition { get; set; }

        public long PriceId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string FrontPhoto { get; set; }

        public string BackPhoto { get; set; }

        public string RearPhoto { get; set; }

        public string LeftSidePhoto { get; set; }

        public string RightSidePhoto { get; set; }

        public List<string> Booktypes { get; set; }

        public string Summary { get; set; }

        public string AudioBookUrl { get; set; }

        public string Author { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool Active { get; set; }
    }
}