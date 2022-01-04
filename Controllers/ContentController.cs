using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueStoryMVC.DataItems.Utils;
using TrueStoryMVC.DataItems.ViewModels;
using TrueStoryMVC.Interfaces.Repository;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Controllers
{
    public class ContentController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISystemRepository _systemRepository;
        public ContentController(
            IPostRepository postRepository,
            IUserRepository userRepository,
            ISystemRepository systemRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _systemRepository = systemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PostRequest  requset)
        {
            if (!ModelState.IsValid) return BadRequest();

            var posts = await _postRepository.GetPostsListAsync(requset.PostBlockType, requset.Number, requset.Argument);
            if (posts == null) return NotFound();

            return PartialView("_GetPosts", posts);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeletePost([FromBody] DeletePostRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userRepository.GetUserAsync(User);
            if (user == null) return NotFound(Failed.ToFindUser());

            var post = await _postRepository.GetPostAsync(request.Id);
            if (post == null) return NotFound(Failed.ToFindPost(request.Id));

            if (post.User.Id != user.Id) 
                if(_userRepository.HasAccessToDelete(User))
                    return Forbid();

            await _postRepository.DeletePost(post);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MakeComment([FromForm] CommentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userRepository.GetUserAsync(User);
            if (user == null) return NotFound(Failed.ToFindUser());

            var post = await _postRepository.GetPostAsync(request.PostId);
            if (post == null) return NotFound(Failed.ToFindPost(request.PostId));

            await _systemRepository.CreateCommentAsync(request, user, post);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like([FromBody] LikeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userRepository.GetUserAsync(User);
            if (user == null) return NotFound(Failed.ToFindUser());

            var likeCount = await _systemRepository.LikeAsync(request, user);
            return Ok(new { result = likeCount });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckLike([FromQuery] LikeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userRepository.GetUserAsync(User);
            if (user == null) return NotFound(Failed.ToFindUser());

            var likeType = _systemRepository.CheckUserLikeType(request, user);
            return Ok(new { result = likeType});
        }
    }
}
