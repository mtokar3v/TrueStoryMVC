using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using TrueStoryMVC.Interfaces.Repositories;
using TrueStoryMVC.Interfaces.Services;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        public AccountRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<User> CreateUserAccount(RegisterUserModel regUserM, ModelStateDictionary modelState) 
        { 
            var user = new User
            {
                UserName = regUserM.Name,
                Email = regUserM.Email,
                RegisterTime = DateTime.UtcNow
            };

            var result =  await _userManager.CreateAsync(user, regUserM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    modelState.AddModelError(string.Empty, error.Description);
                return null;
            }
            return user;
        }

        public async Task SendConfimationMail(User user, IUrlHelper url, HttpContext context)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = url.Action(
               "ConfirmEmail",
                "Account",
                new { userName = user.UserName, code = token },
                context.Request.Scheme);

            var subject = "Confirm your account";
            var message = $"Confirm registration by clicking on the link: <a href='{callbackUrl}'>link</a>";
            await _emailService.SendEmailAsync(user.Email, subject, message);
        }

        public async Task<bool> ConfirmEmail(User user, string code, ModelStateDictionary modelState)
        {
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, true);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    modelState.AddModelError(string.Empty, error.Description);
                }
            }
            return result.Succeeded;
        }

        public async Task<bool> SignIn(User user, string password, bool remeberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, remeberMe, true);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, true);
            }
            return result.Succeeded;
        }

        public Task SignOut() => _signInManager.SignOutAsync();
    }
}
