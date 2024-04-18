using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.ui.Data;
using shopapp.ui.Models;
using shopapp.ui.ViewModels;

namespace shopapp.ui.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var productViewModel = new ProductViewModel()
            {
                Products = ProductRepository.Products
            };
            return View(productViewModel);
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