using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
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

        public IActionResult CreateProduct() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            var entity = new Product() 
            {
                Name = model.Name,
                Url = model.Url,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = model.ImageUrl
            };

            _productService.Create(entity);

            var obj = new AlertMessage()
            {
                Message = $"{entity.Name} named product is created",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(obj);

            return RedirectToAction("ProductList");
        }

        public IActionResult Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            var entity = _productService.GetById((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                Url = entity.Url,
                ImageUrl = entity.ImageUrl,
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Edit(ProductModel model)
        {
            var entity = _productService.GetById(model.ProductId);

            if (entity == null)
            {
                return NotFound();
            }

            entity.ProductId = model.ProductId;
            entity.Name = model.Name;
            entity.Price = model.Price;
            entity.Description = model.Description;
            entity.Url = model.Url;
            entity.ImageUrl = model.ImageUrl;

            _productService.Update(entity);

            var obj = new AlertMessage()
            {
                Message = $"{entity.Name} named product is uptated",
                AlertType = "warning"
            };

            TempData["message"] = JsonConvert.SerializeObject(obj);

            return RedirectToAction("ProductList");
        }


        public IActionResult DeleteProduct(int productId) 
        {
            var entity = _productService.GetById(productId);

            if (entity!=null) 
            {
                _productService.Delete(entity);
            }


            var obj = new AlertMessage()
            {
                Message = $"{entity.Name} named product is deleted",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(obj);

            return RedirectToAction("ProductList");
        }
    }
}
