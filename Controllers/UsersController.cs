using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;
using TrueStoryMVC.Builders;
using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.Extensions.Logging;

namespace TrueStoryMVC.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        private IMemoryCache _cache;
        private readonly ILogger<UsersController> _logger;
        public UsersController(UserManager<User> userManager, IMemoryCache cache, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _cache = cache;
            _logger = logger;
        }
        public IActionResult Index() => View(_userManager.Users.ToList());

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
            if (User.Identity.IsAuthenticated && image.Data != null)
            {
                try
                {
                    User user = await _userManager.FindByNameAsync(User.Identity.Name);
                    ImageBuilder builder = new SquareImageBuilder();
                    builder.SetData(image.Data.ToArray());
                    user.Picture = builder.GetResult();

                    MemoryCacheEntryOptions opt = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                    opt.Priority = CacheItemPriority.High;
                    _cache.Set("Avatar_" + User.Identity.Name, image.Data, opt);

                    await _userManager.UpdateAsync(user);
                }
                catch(Exception ex)
                {
                    _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetAvatar()
        {
            byte[] data = null;
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    if (!_cache.TryGetValue("Avatar_" + User.Identity.Name, out data))
                    {
                        string name = HttpContext.User.Identity.Name;
                        User user = await _userManager.FindByNameAsync(name);
                        data = user?.Picture?.Data;

                        if (data != null)
                        {
                            MemoryCacheEntryOptions opt = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(12));
                            opt.Priority = CacheItemPriority.High;
                            _cache.Set("Avatar_" + User.Identity.Name, data, opt);
                        }
                    }
                }
                catch(Exception ex)
                {
                    if (_cache.Get("Avatar_" + User.Identity.Name) != null)
                        _cache.Remove("Avatar_" + User.Identity.Name);
                    _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
                    return Json(null);
                }
            }
            return Json(new { data = data });
        }

    }
}
