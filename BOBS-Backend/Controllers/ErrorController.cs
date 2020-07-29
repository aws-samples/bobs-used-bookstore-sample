using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BOBS_Backend.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/Index/{code:int}")]
        public IActionResult Index(int code) {


            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewData["Path"] = exception?.Path;
            var error = Problem();

            ViewData["StatusCode"] = code ;
            return View();
        }

        [Route("/error")]
        public IActionResult Support()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewData["Path"] = exception?.Path;
            var error = Problem();

            ViewData["StatusCode"] = error.StatusCode;
            return View("~/Views/Error/Index.cshtml");
        }
    }
}
