using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Book
{
    public class Condition
    {
        [Key]
        public long Condition_Id { get; set; }

        public string ConditionName { get; set; }
    }
}
