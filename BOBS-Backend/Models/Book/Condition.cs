using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Models.Book
{
    public class Condition
    {
        /*
         * Condition Model
         */

        [Key]
        public long Condition_Id { get; set; }

        public string ConditionName { get; set; }
    }
}
