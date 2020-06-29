using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models
{
    public class Condition
    {
        [Required]
        public int ID { get; set; }
        public string condition { get; set; }
    }
}
