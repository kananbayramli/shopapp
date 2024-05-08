﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.ui.Extensions;
using shopapp.ui.Identity;
using shopapp.ui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.ui.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async  Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;  //cool :) 
                list.Add(user);
            }
            var model = new RoleDetails() 
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user!=null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }


                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/role/"+model.RoleId);
        }

            public IActionResult RoleList() 
        {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> RoleCreate( RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await  _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else 
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
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
                    TempData.Put("message", new AlertMessage() 
                    { 
                        Title = "New Product",
                        Message = "Product is create",
                        AlertType = "success"
                    });
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Something went wrong :(",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });

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

                TempData.Put("message", new AlertMessage() 
                {
                    Title = "Shop App Category",
                    Message = $"{entity.Name} named category is created",
                    AlertType = "success"
                });

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
        public async  Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds, IFormFile file)
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

                if (file!=null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }


                
                if (_productService.Update(entity, categoryIds))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Updated",
                        Message = "Product is updated",
                        AlertType = "success"
                    });
                    return RedirectToAction("ProductList");
                }

                TempData.Put("message", new AlertMessage()
                {
                    Title = "Something went wrong :(",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
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


                TempData.Put("message", new AlertMessage()
                {
                    Title = "Shop App Category",
                    Message = $"{entity.Name} named category is uptated",
                    AlertType = "warning"
                });

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


            TempData.Put("message", new AlertMessage()
            {
                Title = "Shop App Product",
                Message = $"{entity.Name} named product is deleted",
                AlertType = "danger"
            });

            return RedirectToAction("ProductList");
        }



        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Shop App Category",
                Message = $"{entity.Name} named category is deleted",
                AlertType = "danger"
            });

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
