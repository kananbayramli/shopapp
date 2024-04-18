using System.Collections.Generic;
using System.Linq;
using shopapp.ui.Models;

namespace shopapp.ui.Data
{
    public static class ProductRepository
    {
        private static List<Product> _products = null;

        static ProductRepository()
        {
            _products = new List<Product>
            {
                new Product{ProductId=1, Name="IPhone 8 plus", Description="Cox ela telefon", Price=1000, IsApproved=true, ImageUrl="1.jpg"},
                new Product{ProductId=2,Name="IPhone 11", Description="Yaxwi telefon", Price=1200, IsApproved=true, ImageUrl="2.jpg"},
                new Product{ProductId=3,Name="Xiami Red Mi", Description="Yaxwi telefon", Price=1200, ImageUrl="3.jpg"},
                new Product{ProductId=4,Name="IPhone 13 Pro", Description="Yaxwi telefon", Price=1200, IsApproved=false, ImageUrl="4.jpg"}

            };
        }

        public static List<Product> Products
        {
            get{
                return _products;
            }
        }

        public static void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public static Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.ProductId == id);
        }
    }
}