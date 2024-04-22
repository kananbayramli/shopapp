using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
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

        public IActionResult List()
        {
            var productViewModel = new ProductListViewModel()
            {
                Products = _productService.GetAll()
            };
            return View(productViewModel);
        }
    }
}