using System.Collections.Generic;
using System.Linq;
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

        public IActionResult List(int? id)
        {
            var products = ProductRepository.Products;
            if(id!=null)
            {
                products = products.Where(p => p.CategoryId == id).ToList();
            }

            var productViewModel = new ProductViewModel()
            {
                Products = products,
            };
            return View(productViewModel);
            
        }

        public IActionResult Details(int id)
        {
            return View(ProductRepository.GetProductById(id));
        }
    }

} 