using Microsoft.AspNetCore.Mvc;

namespace shopapp.ui.Controllers
{

    public class ProductController : Controller
    {
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }

}