using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;
using TrueStoryMVC.Builders;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace TrueStoryMVC.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        private IMemoryCache _cache;
        public UsersController(UserManager<User> userManager, IMemoryCache cache)
        {
            _userManager = userManager;
            _cache = cache;
        }
        public IActionResult Index() => View(_userManager.Users.ToList());

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UserPage(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                User user = new User();
                user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                    return View(user);
                else
                    return NotFound();
            }
            else
                return RedirectToAction("hot");
        }

        [HttpPost]
        public async Task UpdateAvatar([FromBody] OneImage image)
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                ImageBuilder builder = new SquareImageBuilder();
                //ImageConstructor constructor = new ImageConstructor(builder);
                //constructor.ConstructImage(image.Data.ToArray());
                builder.SetData(image.Data.ToArray());
                user.Picture = builder.GetResult();
                await _userManager.UpdateAsync(user);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetAvatar()
        {

            //User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            //byte[] data = new byte[user.Picture.Data.Length];
            //data = user.Picture.Data;
            byte[] data = null;
            if (User.Identity.IsAuthenticated)
            {
                
                if (!_cache.TryGetValue("Avatar", out data))
                {
                    string name = HttpContext.User.Identity.Name;
                    User user = await _userManager.FindByNameAsync(name);
                    data = user.Picture.Data;

                    if (data != null)
                    {
                        MemoryCacheEntryOptions opt = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(12));
                        opt.Priority = CacheItemPriority.High;
                        _cache.Set("avatar", data, opt);
                    }
                }
            }
            return Json(new { data = data });
        }
    }
}
