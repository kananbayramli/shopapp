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
                new Product{ProductId=5, Name="IPhone 8 plus", Description="Cox ela telefon", Price=1000, IsApproved=true, ImageUrl="1.jpg", CategoryId=1},
                new Product{ProductId=6,Name="IPhone 11", Description="Yaxwi telefon", Price=1200, IsApproved=true, ImageUrl="2.jpg", CategoryId=1},
                new Product{ProductId=7,Name="Xiami Red Mi", Description="Yaxwi telefon", Price=1200, ImageUrl="3.jpg", CategoryId=1},
                new Product{ProductId=8,Name="IPhone 13 Pro", Description="Yaxwi telefon", Price=1200, IsApproved=false, ImageUrl="4.jpg", CategoryId=1},


                new Product{ProductId=1, Name="Macbook  plus", Description="Cox ela notebook", Price=1000, IsApproved=true, ImageUrl="5.jpg", CategoryId=2},
                new Product{ProductId=2,Name="Omen 11", Description="Yaxwi masaustu", Price=1200, IsApproved=true, ImageUrl="6.jpg", CategoryId=2},
                new Product{ProductId=3,Name="Acer Grand", Description="Ela Notebook", Price=1200, ImageUrl="6.jpg", CategoryId=2},
                new Product{ProductId=4,Name="Samsung PC", Description="Dozumlu notebook", Price=1200, IsApproved=false, ImageUrl="6.jpg", CategoryId=2}
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

        public static void EditProduct(Product product)
        {
            foreach (var p in _products)
            {
                if(p.ProductId == product.ProductId)
                {
                    p.Name = product.Name;
                    p.Description = product.Description;
                    p.Price = product.Price;
                    p.ImageUrl = product.ImageUrl;
                    p.IsApproved = product.IsApproved;
                    p.CategoryId = product.CategoryId;
                }
            }
        }

        public static void DeleteProduct(int id)
        {
           var product = GetProductById(id);

           if(product != null)
           {
             _products.Remove(product);
           }

        }
    }
}