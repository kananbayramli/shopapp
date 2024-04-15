using Microsoft.AspNetCore.Mvc;

namespace shopapp.ui.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }

}