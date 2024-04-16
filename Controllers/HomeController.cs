using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.ui.Models;
using shopapp.ui.ViewModels;

namespace shopapp.ui.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<Product>()
            {
                new Product{Name="IPhone 8 plus", Description="Cox ela telefon", Price=1000, IsApproved=true},
                new Product{Name="IPhone 11", Description="Yaxwi telefon", Price=1200, IsApproved=true},
                new Product{Name="IPhone 13 Pro", Description="Yaxwi telefon", Price=1200},
                new Product{Name="IPhone 13 Pro", Description="Yaxwi telefon", Price=1200, IsApproved=false}
            };

            
            var productViewModel = new ProductViewModel()
            {
                Products = products,
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