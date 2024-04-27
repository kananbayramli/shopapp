using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.ui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.ui.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        public AdminController(IProductService productService)
        {
            _productService = productService;
        }


        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }
    }
}
