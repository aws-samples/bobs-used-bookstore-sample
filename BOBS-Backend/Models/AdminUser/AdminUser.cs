using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace BOBS_Backend.Models.AdminUser
{
    public class AdminUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string group { get; set; }

    }

    public class CreateUser
    {
       
    }
   
}
