using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers
{
    public class ExternalAuthenticationController : Controller
    {
        public IActionResult Callback()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
