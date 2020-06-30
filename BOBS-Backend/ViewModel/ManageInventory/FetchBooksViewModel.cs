using Amazon.Auth.AccessControlPolicy;
using BOBS_Backend.DataModel;
using BOBS_Backend.Models;
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

        public Condition condition { get; set; }

        public BOBS_Backend.Models.Book.Type BookType { get; set; }

        public BOBS_Backend.Models.Book.Publisher publisher { get; set; }

        public IEnumerable<BookDetails> Books { get; set; }

    }
}
