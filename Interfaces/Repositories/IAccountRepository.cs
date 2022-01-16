using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<User> CreateUserAccount(RegisterUserModel regUserM, ModelStateDictionary modelState);
        Task SendConfimationMail(User user, IUrlHelper url, HttpContext context);
        Task<bool> ConfirmEmail(User user, string code, ModelStateDictionary modelState);
        Task<bool> SignIn(User user, string password, bool remeberMe);
        Task SignOut();
    }
}
