using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BOBS_Backend.Models.Customer
{
    public class Customer
    {
        /*
         * Customer Model
         */

        [Key]
        [StringLength(450)]
        public string Customer_Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; }

    }
}
