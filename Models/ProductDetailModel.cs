using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.ui.Models
{
    public class ProductDetailModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set;}
    }
}