using System.Collections.Generic;
using System.Linq;
using shopapp.ui.Models;

namespace shopapp.ui.Data
{
    public class CategoryRepository
    {
        private static List<Category> _categories = null;

        static CategoryRepository()
        {
            _categories = new List<Category>
            {
                new Category{CategoryId=1, Name="Telefon", Description="Telefonlar"},
                new Category{CategoryId=2, Name="Notebook", Description="Noutbooklar"},
                new Category{CategoryId=3, Name="Electronik", Description="Elektronikler"}
            };
        }

        public static List<Category> Categories
        {
            get
            {
                return _categories;
            }
        }

        public static void AddCategory(Category category)
        {
            _categories.Add(category);
        }

        public static Category GetCategoryById(int id)
        {
            return _categories.FirstOrDefault(c=> c.CategoryId == id);
        }
    }
}