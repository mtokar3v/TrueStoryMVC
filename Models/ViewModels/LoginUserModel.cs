using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models.ViewModels
{
    public class LoginUserModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; } //passwordSingIn принимает в качестве первого аргумента string Name, что вообще то является логином

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}
