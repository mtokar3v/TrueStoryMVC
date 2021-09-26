﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TrueStoryMVC.Models;
using System.Collections.Generic;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
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
                user.Picture.Data = image.Data.ToArray();
                await _userManager.UpdateAsync(user);

            }
        }
    }
}
