using BOBS_Backend.Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.ViewModel.UpdateBooks
{
    public class BookUpdates
    {
        public List<Price> books { get; set; }
        public List<Price> globalBooks { get; set; }
    }
}
