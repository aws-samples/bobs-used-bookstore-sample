using System;
using System.Collections.Generic;
using BobsBookstore.Models.Books;
using BobsBookstore.Models.Customers;
using Microsoft.AspNetCore.Http;

namespace BookstoreBackend.ViewModel.ResaleBooks
{
    public class ResaleViewModel
    {
        public long Resale_Id { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string GenreName { get; set; }
        public string PublisherName { get; set; }
        public string TypeName { get; set; }
        public ResaleStatus ResaleStatus { get; set; }
        public Customer Customer { get; set; }
        public decimal BookPrice { get; set; }
        public string ConditionName { get; set; }
        public IFormFile FrontUrl { get; set; }
        public IFormFile BackUrl { get; set; }
        public IFormFile LeftUrl { get; set; }
        public IFormFile RightUrl { get; set; }
        public int Quantity { get; set; }
        public bool Active { get; set; }



    }
}
