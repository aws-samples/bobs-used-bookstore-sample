using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Models.Book
{
    public class Genere
    {

        [Key]
        public long Genere_Id { get; set; }

        public string Name { get; set; }
    }
}
