using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Book
{
    public class Price
    {
        [Key]
        public long Price_Id { get; set; }

        public Book Book { get; set; }

        public Condition Condition { get; set; }

        public double ItemPrice { get; set; }

        public int Quantity { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
