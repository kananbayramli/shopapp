using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.ui.Data;
using shopapp.ui.Models;

namespace shopapp.ui.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var categories = CategoryRepository.Categories;
            return View(categories);
        }
    }
}