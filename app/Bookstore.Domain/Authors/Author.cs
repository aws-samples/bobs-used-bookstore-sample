using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Authors
{
    public class Author
    {
        [Key]
        public int BusinessEntityID { get; set; }

        [Required]
        [StringLength(15)]
        public string NationalIDNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string LoginID { get; set; }

        [Required]
        [StringLength(50)]
        public string JobTitle { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(1)]
        public string MaritalStatus { get; set; }

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public short VacationHours { get; set; }
        
        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
