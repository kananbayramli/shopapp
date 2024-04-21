using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace shopapp.ui.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // if(RouteData.Values["action"].ToString() == "List")
            //     ViewBag.SelectedCategory = RouteData?.Values["id"];
                
            // var categories = CategoryRepository.Categories;
            // return View(categories);
            return View();
        }
    }
}