using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.ui.ViewModels;

namespace shopapp.ui.Controllers
{

    public class HomeController : Controller
    {
        private IProductService _productService;

        public HomeController(IProductService productService)
        {
            this._productService = productService;
        }

        public IActionResult Index()
        {
            var productViewModel = new ProductViewModel()
            {
                Products = _productService.GetAll()
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