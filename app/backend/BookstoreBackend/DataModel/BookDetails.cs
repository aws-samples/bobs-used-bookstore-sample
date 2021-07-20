﻿using System;
using Microsoft.AspNetCore.Http;
using BobsBookstore.Models.Books;
using Type = BobsBookstore.Models.Books.Type;

namespace BookstoreBackend.DataModel
{
    public class BookDetails
    {
        public string BookName { get; set; }

        public long BookId { get; set; }

        public Publisher Publisher { get; set; }

        public Condition BookCondition { get; set; }

        public Type BookType { get; set; }

        public double Price { get; set; }

        public Genre Genre { get; set; }

        public int Quantity { get; set; }

        public string front_url { get; set; }

        public string back_url { get; set; }

        public string left_url { get; set; }

        public string right_url { get; set; }

        public string BookNameSearchParameter { get; set; }

        public IFormFile FrontPhoto { get; set; }
        public IFormFile BackPhoto { get; set; }
        public IFormFile RearPhoto { get; set; }
        public IFormFile LeftSidePhoto { get; set; }
        public IFormFile RightSidePhoto { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Author { get; set; }

        public bool Active { get; set; }

        public string Summary { get; set; }

        public long ISBN { get; set; }
    }
}
