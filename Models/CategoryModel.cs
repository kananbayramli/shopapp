using shopapp.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.ui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Kateqoriya adini daxil edin")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Kateqoriya adi ucun 5-100 arasinda xarakter qebul olunur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kateqoriya url-ni daxil edin")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Kateqoriya url-i ucun 5-100 arasinda xarakter qebul olunur.")]
        public string Url { get; set; }

        public List<Product> Products { get; set; }
    }
}
