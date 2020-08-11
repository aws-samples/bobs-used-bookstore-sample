using BOBS_Backend.Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.ViewModel.SearchBooks
{
    public class FullBook
    {

        public int LowestPrice { get; set; }

        public int TotalQuantity { get; set; }

        public Price Price { get; set; }
    }
}
