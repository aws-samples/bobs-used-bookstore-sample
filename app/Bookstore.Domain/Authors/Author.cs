using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Authors
{
    [Table("author", Schema = "public")]
    public class Author
    {
        [Key]
        [Column("business_entity_id")]
        public int BusinessEntityID { get; set; }

        [Required]
        [StringLength(15)]
        [Column("national_id_number")]
        public string NationalIDNumber { get; set; }

        [Required]
        [StringLength(256)]
        [Column("login_id")]
        public string LoginID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("job_title")]
        public string JobTitle { get; set; }

        [Required]
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(1)]
        [Column("marital_status")]
        public string MaritalStatus { get; set; }

        [Required]
        [StringLength(1)]
        [Column("gender")]
        public string Gender { get; set; }

        [Required]
        [Column("hire_date")]
        public DateTime HireDate { get; set; }

        [Required]
        [Column("vacation_hours")]
        public short VacationHours { get; set; }
        
        [Required]
        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
