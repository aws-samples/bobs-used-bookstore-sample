using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Book
{
    public class Genre
    {
        [Key]
        public long Genre_Id { get; set; }

        public string Name { get; set; }
    }
}
