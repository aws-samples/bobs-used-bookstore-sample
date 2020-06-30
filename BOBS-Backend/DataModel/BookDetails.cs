using BOBS_Backend.Models;
using BOBS_Backend.Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.DataModel
{
    public class BookDetails
    {
        public string BookName { get; set; }

        public string Publisher { get; set; }

        public Condition BookCondition { get; set; }

        public Models.Book.Type BookType { get; set; }

        public double Price { get; set; }

        public Genre Genre { get; set; }

        public int Quantity { get; set; }

        public string front_url { get; set; }

        public string back_url { get; set; }

        public string left_url { get; set; }

        public string right_url { get; set; }

        public string BookNameSearchParameter { get; set; }

    }
}
