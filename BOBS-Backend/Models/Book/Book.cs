using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Models.Book
{
    public class Book
    {
        [Key]
        public long Book_Id { get; set; }

        public Publisher Publisher { get; set; }

        public long ISBN { get; set; }
        
        public Type Type { get; set; }

        public string Name { get; set; }

        public Genre Genre { get; set; }



    }

}
