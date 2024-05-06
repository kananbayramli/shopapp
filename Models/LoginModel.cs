using System.ComponentModel.DataAnnotations;

namespace shopapp.ui.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        // [Display(Name = "Username")]
        // public string UserName { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}