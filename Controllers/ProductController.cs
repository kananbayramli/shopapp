using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult List(int? id, string q)
        {
            var products = ProductRepository.Products;
            if(id!=null)
            {
                products = products.Where(p => p.CategoryId == id).ToList();
            }

            if(!string.IsNullOrEmpty(q))
            {
                products = products.Where(p => p.Name.ToLower().Contains(q) || p.Description.ToLower().Contains(q)).ToList();
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



        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product p)
        {
            if(ModelState.IsValid)
            {
                ProductRepository.AddProduct(p);
                return RedirectToAction("list");
            }
            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");
            return View(p);
        }

        public IActionResult Edit(int id)
        { 
            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");
            return View(ProductRepository.GetProductById(id));
        }

        [HttpPost]
        public IActionResult Edit(Product p)
        {
            ProductRepository.EditProduct(p);
            return RedirectToAction("list");
        }


        [HttpPost]
        public IActionResult Delete(int ProductId)
        {
            ProductRepository.DeleteProduct(ProductId);
            return RedirectToAction("list");
        }
    }

} 