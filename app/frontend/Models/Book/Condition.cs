using System.ComponentModel.DataAnnotations;

namespace BobBookstore.Models.Book
{
    public class Condition
    {
        [Key]
        public long Condition_Id { get; set; }

        public string ConditionName { get; set; }
    }
}
