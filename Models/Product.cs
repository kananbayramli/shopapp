using System.ComponentModel.DataAnnotations;

namespace shopapp.ui.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Name is required field !")]
        public string Name { get; set;}
        [Required(ErrorMessage = "Price is required field !")]
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        [Required]
        public int? CategoryId { get; set; }
    }
}