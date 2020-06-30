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
        [Required]
        public int ID { get; set; }
        public int PublisherID { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public int PriceID { get; set; }
        public int ISBN { get; set; }
    }
}
