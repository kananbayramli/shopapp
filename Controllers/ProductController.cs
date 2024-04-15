using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
            var products = new List<Product>()
            {
                new Product{Name="IPhone 8 plus", Description="Cox ela telefon", Price=1000},
                new Product{Name="IPhone X", Description="Yaxwi telefon", Price=1200}
            };
            var category = new Category {Name = "Telefonlar", Description="Telefon Kataloq"};
            
            var productViewModel = new ProductViewModel()
            {
                Products = products,
                Category = category
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