using System;
using Microsoft.AspNetCore.Mvc;

namespace shopapp.ui.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            int saat = DateTime.Now.Hour;
            string mesaj = saat < 12 ? "Sabahiniz xeyir" : "Her vaxtiniz xeyir";
            ViewBag.Greeting = mesaj;
            ViewBag.UserName = "Kenan";
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View("MyView");
        }
    }

}