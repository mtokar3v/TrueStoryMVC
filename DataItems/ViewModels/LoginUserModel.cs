using System.ComponentModel.DataAnnotations;

namespace TrueStoryMVC.Models.ViewModels
{
    public class LoginUserModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
