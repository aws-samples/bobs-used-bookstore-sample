using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace BOBS_Backend.Models.Book
{
    public class Price
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Price_Id { get; set; }
        
        public Book Book { get; set; }

        public Condition Condition { get; set; }

        public double ItemPrice { get; set; }

        public int Quantiy { get; set; }
        public string UpdatedBy { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime UpdatedOn { get; set; }
    }
}
