using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Book
{
    public class Type
    {
        [Key]
        public long Type_Id { get; set; }

        public string TypeName { get; set; }
    }
}
