using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _db;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationContext dbContext, ILogger<HomeController> logger, SignInManager<User> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = dbContext;
            _logger = logger;
            _signInManager = signInManager;
        }

        public IActionResult Index() => View(_roleManager.Roles.ToList());

        public IActionResult CreateRole() => View();

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string UserId)
        {
            User user = await _userManager.FindByIdAsync(UserId);

            if(user != null)
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                List<IdentityRole> AllRoles = _roleManager.Roles.ToList();

                ChangeRoleView changeRole = new ChangeRoleView
                {
                    UserId = user.Id,
                    AllRoles = AllRoles,
                    UserRoles = userRoles,
                    UserName = user.UserName
                };

                return View(changeRole);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);

            if(user!=null)
            {
                IList<string> beforeRoles = await _userManager.GetRolesAsync(user);

                IEnumerable<String> addedRoles = roles.Except(beforeRoles);
                IEnumerable<string> removedRoles = beforeRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }

        [Authorize (Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> UserList(string key, int page = 1)
        {
            int maxUsersOnPage = 2;

            IQueryable<ShortUserInfoModel> source;

            if (string.IsNullOrEmpty(key))
            {
                source = _db.Users.Select(u => new ShortUserInfoModel
                {
                    UserName = u.UserName,
                    Id = u.Id,
                    Avatar = u.Picture.Data
                });
            }
            else
            {
                source = _db.Users.Where(u=>u.UserName.Contains(key)).Select(u => new ShortUserInfoModel
                {
                    UserName = u.UserName,
                    Id = u.Id,
                    Avatar = u.Picture.Data
                });
            }

            int totalUser = await source.CountAsync();

            List<ShortUserInfoModel> users = await source.Skip((page - 1) * maxUsersOnPage).Take(maxUsersOnPage).ToListAsync();

            PageListScrollingModel pageListScrolling = new PageListScrollingModel(page, totalUser, maxUsersOnPage);

            PageListModel<ShortUserInfoModel, string> pageList = new PageListModel<ShortUserInfoModel, string>(users, pageListScrolling, key);

            return  View(pageList);
        }

        [HttpPost]
        public IActionResult UserList(string key)
        {
            return RedirectToAction("userlist", "admin", new { key = key, page = 1});
        }

        [HttpGet]
        [Authorize]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                if (model.Password == "admin")
                {
                    User user = await _userManager.FindByNameAsync(model.Login);
                    if (!await _roleManager.RoleExistsAsync("admin"))
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _userManager.AddToRoleAsync(user, "admin");
                    return RedirectToAction("userlist", "admin");
                }
                ModelState.AddModelError("", "Неверный пароль");
            }
            return View(model);
        }

        //public async Task<IActionResult> DeleteUser(string Name)
        //{
        //    try
        //    {
        //        User user = await _userManager.FindByNameAsync(Name);
        //        if (user != null)
        //        {
        //            user.isEnable = false;
        //            _userManager.
        //        }
        //        else
        //            throw new Exception("Пользователь не найден");
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
        //    }
        //}
    }
}
