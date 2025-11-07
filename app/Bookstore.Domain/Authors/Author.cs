using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Authors
{
    [Table("author", Schema = "bobsusedbookstore_dbo")]
    public class Author
    {
        [Key]
        [Column("businessentityid")]
        public int BusinessEntityID { get; set; }

        [Required]
        [StringLength(15)]
        [Column("nationalidnumber")]
        public string NationalIDNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        [Column("loginid")]
        public string LoginID { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("jobtitle")]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [Column("birthdate")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(1)]
        [Column("maritalstatus")]
        public string MaritalStatus { get; set; } = string.Empty;

        [Required]
        [StringLength(1)]
        [Column("gender")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [Column("hiredate")]
        public DateTime HireDate { get; set; }

        [Required]
        [Column("vacationhours")]
        public short VacationHours { get; set; }
        
        [Required]
        [Column("modifieddate")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
