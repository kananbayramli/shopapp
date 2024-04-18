using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.ui.Data;
using shopapp.ui.Models;
using shopapp.ui.ViewModels;

namespace shopapp.ui.Controllers
{

    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var product = new Product{Name="IPhone 12", Price= 2000, Description="Ela Telefon"};
            ViewData["Category"] = "Telefon";
            // ViewData["Product"] = product;
            return View(product);
        }

        public IActionResult List()
        {     
            var productViewModel = new ProductViewModel()
            {
                Products = ProductRepository.Products,
            };
            return View(productViewModel);
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