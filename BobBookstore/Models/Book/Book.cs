using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Book
{
    public class Book
    {
        [Key]
        public long Book_Id { get; set; }

        public Publisher Publisher { get; set; }

        public long ISBN { get; set; }

        public Type Type { get; set; }

        public string Name { get; set; }

        public virtual Genre Genre { get; set; }

        public string Front_Url { get; set; }

        public string Back_Url { get; set; }

        public string Left_Url { get; set; }

        public string Right_Url { get; set; }

        public string AudioBook_Url { get; set; }

        public string Summary_Url { get; set; }
    }
}
