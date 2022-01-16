using System.ComponentModel.DataAnnotations;

namespace TrueStoryMVC.Models.ViewModels
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage ="Enter Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Enter userName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter password again")]
        [Compare("Password", ErrorMessage = "Passwords has not compare")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm a password")]
        public string PasswordConfirm { get; set; }
    }
}
