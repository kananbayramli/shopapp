using System.Linq;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.ui.Models;
using shopapp.ui.ViewModels;

namespace shopapp.ui.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }

        public IActionResult List(string category)
        {
            var productViewModel = new ProductListViewModel()
            {
                Products = _productService.GetProductsByCategory(category)
            };
            return View(productViewModel);
        }

        public IActionResult Details(string url)
        {
            if(url == null)
            {
                return NotFound();
            }

            Product product = _productService.GetProductDetails(url);
            if(product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailModel{
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList()
            });
        }

    }
}