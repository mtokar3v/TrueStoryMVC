using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrueStoryMVC.Models.ViewModels;
using System.Threading.Tasks;
using TrueStoryMVC.Interfaces.Repository;
using TrueStoryMVC.DataItems.Utils;

namespace TrueStoryMVC.Controllers
{
    public class ConstructorController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        public ConstructorController(
            IPostRepository postRepository,
            IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> CreatePost([FromBody] PostModel postModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userRepository.GetUserAsync(User);
            if (user == null) return NotFound(Failed.ToFindUser());

            await _postRepository.CreatePost(postModel, user);
            return Ok();
        } 
    }
}
