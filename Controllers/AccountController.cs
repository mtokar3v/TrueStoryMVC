using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrueStoryMVC.DataItems.Utils;
using TrueStoryMVC.Interfaces.Repositories;
using TrueStoryMVC.Interfaces.Repository;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;


        public AccountController(
            IAccountRepository accountRepository,
            IUserRepository userRepository
            )
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _accountRepository.CreateUserAccount(model, ModelState);
            if (user == null) return View(model);

            await _accountRepository.SendConfimationMail(user, Url, HttpContext);
            return Content("To complete the registration, check your email and follow the link provided in the letter");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userName, string code)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(code)) return BadRequest();

            var user = await _userRepository.GetUserAsync(userName);
            if (user == null) return NotFound();

            var hasConfirmEmail = await _accountRepository.ConfirmEmail(user, code, ModelState);
            if (!hasConfirmEmail) return View(user);

            return RedirectToAction("Hot", "Content");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var user = await _userRepository.GetUserAsync(model.Login);
            if (user == null) 
            {
                ModelState.AddModelError("", Failed.ToFindUser(model.Login));
                return View(model); 
            };

            var canSignIn = await _accountRepository.SignIn(user, model.Password, model.RememberMe);
            if (!canSignIn)
            {
                ModelState.AddModelError("", Failed.ToSignIn());
                return View(model);
            }

            return RedirectToAction("Hot", "Content");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignOut();
            return RedirectToAction("Hot", "Content");
        }
    }
}
