using System.ComponentModel.DataAnnotations;

namespace DataModels.Books
{
    public class ResaleStatus
    {
        [Key] public long ResaleStatus_Id { get; set; }

        public string Status { get; set; }
    }
}