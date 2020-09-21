using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBS_Backend.Models.Book
{
    public class Type
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Type_Id { get; set; }

        public string TypeName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
