using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.data.Abstract;
using shopapp.ui.ViewModels;

namespace shopapp.ui.Controllers
{

    public class HomeController : Controller
    {
        private IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var productViewModel = new ProductViewModel()
            {
                Products = _productRepository.GetAll()
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