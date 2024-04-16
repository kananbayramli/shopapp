using System.Collections.Generic;
using shopapp.ui.Models;

namespace shopapp.ui.ViewModels
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}