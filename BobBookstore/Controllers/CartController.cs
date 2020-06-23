using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BobBookstore.Controllers
{
    [Authorize(Roles = "Users")]
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
