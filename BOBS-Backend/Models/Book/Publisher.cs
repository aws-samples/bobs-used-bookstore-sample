using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Models.Book
{
    public class Publisher
    {
        /*
         * Publisher Model
         */
        [Key]
        public long Publisher_Id { get; set; }

        public string Name { get; set; }

    }
}
