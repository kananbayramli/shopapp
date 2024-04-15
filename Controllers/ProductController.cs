using Microsoft.AspNetCore.Mvc;
using shopapp.ui.Models;

namespace shopapp.ui.Controllers
{

    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var p = new Product();
            p.Name = "Samsung s6";
            p.Price = 2000;
            p.Description = "Gozel Telefondur";
            return View(p);
        }
    }

} 