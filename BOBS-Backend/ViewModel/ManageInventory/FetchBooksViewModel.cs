using Amazon.Auth.AccessControlPolicy;
using BOBS_Backend.DataModel;
using BOBS_Backend.Models;
using BOBS_Backend.Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Condition = BOBS_Backend.Models.Book.Condition;

namespace BOBS_Backend.ViewModel
{
    public class FetchBooksViewModel
    {
        public string BookName { get; set; }
        public string publisher { get; set; }
        public string BookType { get; set; }
        public string genre { get; set; }
        public string Condition { get; set; }

        public IEnumerable<BookDetails> Books { get; set; }

        public string searchfilter { get; set; }

        public string searchby { get; set; }

        public string front_url { get; set; }

        public string back_url { get; set; }

        public string left_url { get; set; }

        public string right_url { get; set; }

        public string typechosen { get; set; }

        public string ViewStyle { get; set; }

        public string type {get;set;}

        public string condition_chosen { get; set; }


        public string Author { get; set; }

    }
}
