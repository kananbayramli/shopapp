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
        private readonly ICategoryService _categoryService;
        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }


        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            });
        }

        public IActionResult ProductCreate() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProductCreate(ProductModel model)
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



        public IActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name,
                Url = model.Url
            };

            _categoryService.Create(entity);

            var obj = new AlertMessage()
            {
                Message = $"{entity.Name} named category is created",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(obj);

            return RedirectToAction("CategoryList");
        }






        public IActionResult ProductEdit(int? id)
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
        public IActionResult ProductEdit(ProductModel model)
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







        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.CategoryId);

            if (entity == null)
            {
                return NotFound();
            }

            entity.CategoryId = model.CategoryId;
            entity.Name = model.Name;
            entity.Url = model.Url;

            _categoryService.Update(entity);

            var obj = new AlertMessage()
            {
                Message = $"{entity.Name} named category is uptated",
                AlertType = "warning"
            };

            TempData["message"] = JsonConvert.SerializeObject(obj);

            return RedirectToAction("CategoryList");
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







        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if (entity != null)
            {
                _categoryService.Delete(entity);
            }


            var obj = new AlertMessage()
            {
                Message = $"{entity.Name} named category is deleted",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(obj);

            return RedirectToAction("CategoryList");
        }





        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId) 
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/"+categoryId);
        }


    }
}
