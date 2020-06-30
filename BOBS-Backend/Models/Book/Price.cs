using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Models.Book
{
    public class Price
    {
        /*
         * Price Model
         * Object reference indicate relationhsip to other tables
         */
        [Key]
        public long Price_Id { get; set; }
        
        // Many-to-One Relationship with Book Table
        public Book Book { get; set; }

        // Many-to-One Relationship with Condition Table
        public Condition Condition { get; set; }

        public double ItemPrice { get; set; }

        public int Quantiy { get; set; }
    }
}
