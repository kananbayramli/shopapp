using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.ui.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Display(Name="Name", Prompt ="Enter product name")]
        public string Name { get; set; }

        [Display(Name = "Url", Prompt = "Enter unique url name")]
        public string Url { get; set; }

        [Display(Name = "Price", Prompt = "Enter product price")]
        public decimal? Price { get; set; }

        [Display(Name = "Description", Prompt = "Enter your description")]
        public string Description { get; set; }

        [Display(Name = "Image", Prompt = "Add image url")]
        public string ImageUrl { get; set; }

        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
    }
}
