using Microsoft.AspNetCore.Mvc;

namespace BobBookstore.Controllers
{
    public class RecommendController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}