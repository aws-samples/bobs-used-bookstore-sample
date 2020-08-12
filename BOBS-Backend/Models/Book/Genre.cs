using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace BOBS_Backend.Models.Book
{
    public class Genre
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Genre_Id { get; set; }

        public string Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
