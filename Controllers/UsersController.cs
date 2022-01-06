using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;
using TrueStoryMVC.Builders;
using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.Extensions.Logging;
using TrueStoryMVC.Interfaces.Repository;
using TrueStoryMVC.DataItems.Utils;
using Microsoft.AspNetCore.Authorization;

namespace TrueStoryMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISystemRepository _systemRepository;
        public UsersController(
            IUserRepository userRepository,
            ISystemRepository systemRepository
            )
        {
            _systemRepository = systemRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> UserPage(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return BadRequest();

            var user = await _userRepository.GetUserAsync(userName);
            if (user == null) return NotFound(Failed.ToFindUser(userName));

            return View(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateAvatar([FromBody] ChangeAvatarRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userRepository.GetUserAsync(User);
            if (user == null) return NotFound(Failed.ToFindUser());

            await _userRepository.UpdateAvatar(request, user);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAvatar()
        {
            var user = await _userRepository.GetUserAsync(User);
            if (user == null) return NotFound(Failed.ToFindUser());

            var avatar = user?.Picture?.Data ?? null;
            return Ok(new { data = avatar });
        }

    }
}
