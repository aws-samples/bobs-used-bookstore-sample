using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.Models.Book;

namespace BOBS_Backend.ViewModel.UpdateBooks
{
    public class UpdatedBooksModel
    {
        public string BookName { get; set; }

        public long BookId { get; set; }

        public Publisher Publisher { get; set; }

        public Condition BookCondition { get; set; }

        public Models.Book.Type BookType { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public double Price { get; set; }

        public Genre Genre { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

    }
}
