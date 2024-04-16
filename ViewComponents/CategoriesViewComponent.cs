using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.ui.Models;

namespace shopapp.ui.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var categories = new List<Category>()
            {
                new Category{Name="Telefon", Description="Telefonlar"},
                new Category{Name="Notebook", Description="Noutbooklar"},
                new Category{Name="Electronik", Description="Elektronikler"}
            };
            return View(categories);
        }
    }
}