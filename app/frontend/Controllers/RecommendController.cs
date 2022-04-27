using Microsoft.AspNetCore.Mvc;

namespace BookstoreFrontend.Controllers
{
    public class RecommendController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
