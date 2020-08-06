using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Book
{
    public class Price
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Price_Id { get; set; }

        public Book Book { get; set; }

        public Condition Condition { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public double ItemPrice { get; set; }

        public int Quantity { get; set; }
        public string UpdatedBy { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime UpdatedOn { get; set; }

        public bool Active { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
