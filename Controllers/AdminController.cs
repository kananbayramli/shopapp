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
            if (ModelState.IsValid) 
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Price = model.Price,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };

                if (_productService.Create(entity))
                {
                    CreateMessage("Product is create", "success");
                    return RedirectToAction("ProductList");
                }
                CreateMessage(_productService.ErrorMessage, "danger");
                return View(model);
            }
            return View(model);
        }



        public IActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            if (ModelState.IsValid)
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
            return View(model);
        }






        public IActionResult ProductEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

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
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }


        [HttpPost]
        public IActionResult ProductEdit(ProductModel model, int[] categoryIds)
        {
            if (ModelState.IsValid)
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
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;
                entity.ImageUrl = model.ImageUrl;

                
                if (_productService.Update(entity, categoryIds))
                {
                    CreateMessage("Product is updated", "success");
                    return RedirectToAction("ProductList");
                }
                CreateMessage(_productService.ErrorMessage, "danger");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
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
            if (ModelState.IsValid)
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
            return View(model);
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


        private void CreateMessage(string message, string alerttype) 
        {
            var obj = new AlertMessage()
            {
                Message = message,
                AlertType = alerttype
            };

            TempData["message"] = JsonConvert.SerializeObject(obj);
        }


    }
}
