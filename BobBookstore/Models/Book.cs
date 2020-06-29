using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models
{
    public class Book
    {
        [Key]
        public int Book_Id { get; set; }
        public Publisher Publisher { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public Genre Genre { get; set; }
        public long ISBN { get; set; }
    }
}
