using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using TrueStoryMVC.Builders;
using WebApiLib.Interfaces.Repositories;
using TrueStoryMVC.Services;

namespace TrueStoryMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly RootDb _db;
        private readonly IMemoryCache _cache;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepository _postRepository;
        public HomeController(
            RootDb dbContext,
            UserManager<User> userManager,
            ILogger<HomeController> logger,
            IMemoryCache cache,
            IPostRepository postRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _db = dbContext;
            _cache = cache;
            _postRepository = postRepository;
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] PostModel postModel)
        {
            if (User.Identity.IsAuthenticated && postModel != null)
            {
                if (postModel.Validate().IsValid)
                {
                    try
                    {
                        User user = null;
                        if (!_cache.TryGetValue(HttpContext.User.Identity.Name, out user))
                        {
                            user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                            if (user != null)
                            {
                                MemoryCacheEntryOptions opt = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(12));
                                opt.Priority = CacheItemPriority.High;
                                _cache.Set(user.UserName, user, opt);
                            }
                        }

                        if (user == null)
                            return BadRequest();

                        Post post = new Post { Header = postModel.Header, PostTime = DateTime.Now.ToUniversalTime(), Author = User.Identity.Name, Tags = postModel.TagsLine, Scheme = postModel.Scheme };
                        _db.Posts.Add(post);
                        await _db.SaveChangesAsync();

                        foreach (var t in postModel.Texts)
                        {
                            Text text = new Text { PostId = post.Id, TextData = t };
                            _db.Texts.Add(text);
                        }

                        foreach (var i in postModel.Images)
                        {
                            PostPicture pic = new PostPicture();
                            ImageBuilder builder = new RectImageBuilder();
                            builder.SetData(i.ToArray());
                            pic.Picture = builder.GetResult();
                            pic.PostId = post.Id;
                            _db.Pictures.Add(pic);
                        }

                        user.PostCount++;
                        _db.Users.Update(user);

                        await _db.SaveChangesAsync();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
                    }
                }
                else
                    _logger.LogInformation("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, postModel.Validate().ErrorList);
            }
            return BadRequest();
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    Post post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == id);
                    if (post != null)
                    {
                        _db.Posts.Remove(post);
                        await _db.SaveChangesAsync();
                    }
                    else
                        return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
                }
            }
            return RedirectToAction("Hot");
        }

        public IActionResult Hot()
        {
            return View();
        }

        public IActionResult Best()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }
        public IActionResult Tag(string SomeTags)
        {
            return View("Tag",SomeTags);
        }

        [HttpPost]
        public async Task<IActionResult> GetPostBlock([FromBody] PostBlockInfo postBlock)
        {
            if (!ModelState.IsValid) return BadRequest();

            var posts = await _postRepository.GetPostsListAsync((DataItems.PostTypes)postBlock.PostBlockType, postBlock.Number, postBlock.Argument);
            if (posts == null) return NotFound();

            return PartialView("_GetPosts", posts);
            //const byte HOT = 0;
            //const byte BEST = 1;
            //const byte NEW = 2;
            //const byte TAGS = 3;
            //const byte USER = 4;

            //List<Post> posts;
            //try
            //{
            //    IPostGetter postGetter = postBlock.PostBlockType switch
            //    {
            //        HOT => new HotPostGetter(),
            //        BEST => new BestPostGetter(),
            //        NEW => new NewPostGetter(),
            //        TAGS => new TagsPostGetter(),
            //        USER => new UserPostGetter(),
            //        _ => throw new Exception("Неизвестный PostBlockType")
            //    };

            //    posts = await postGetter.GetPosts(_db, postBlock).ToListAsync();

            //    if (posts == null)
            //        return Ok();

            //    foreach (var item in posts)
            //    {
            //        await _db.Texts.Where(i => i.PostId == item.Id).LoadAsync();
            //        await _db.Pictures.Where(i => i.PostId == item.Id).LoadAsync();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
            //    posts = null;
            //}
            //return PartialView("_GetPosts", posts);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Comment([FromForm] CommentModel comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    User user = null;
                    if (!_cache.TryGetValue(HttpContext.User.Identity.Name, out user))
                        user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null) user.CommentCount++; else return Unauthorized();

                    Comment newComment = new Comment { Text = comment.Text, Author = HttpContext.User.Identity.Name, PostTime = DateTime.Now.ToUniversalTime(), PostId = comment.PostId };
                    
                    MemoryCacheEntryOptions opt = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                    opt.Priority = CacheItemPriority.High;
                    _cache.Set(user.UserName, user, opt);
                    _db.Users.Update(user);
                    _db.Comments.Add(newComment);
                    await _db.SaveChangesAsync();

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
                    return BadRequest();
                }
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<JsonResult> Like([FromBody] LikeModel like)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                const byte FROM_POST = 0;
                const byte FROM_COMMENT = 1;

                int result;
                try
                {
                    User user = null;
                    user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

                    Like _like = like.FromType switch
                    {
                        FROM_POST => await _db.Likes.FirstOrDefaultAsync(l => l.PostId == like.PostId && l.UserId == user.Id),
                        FROM_COMMENT => await _db.Likes.FirstOrDefaultAsync(l => l.CommentId == like.PostId && l.UserId == user.Id),
                        _ => throw new Exception("unknown like type")
                    };

                    if (_like == null)
                    {
                        _like = new Like { LikeType = like.LikeType, UserId = user.Id };

                        if (like.FromType == FROM_POST)
                            _like.PostId = like.PostId;
                        else if (like.FromType == FROM_COMMENT)
                            _like.CommentId = like.PostId;
                        else
                            throw new Exception("unknown post type");

                        _db.Likes.Add(_like);
                        await _db.SaveChangesAsync();
                        result = _like.likeCalculate(1);
                    }
                    else if (_like.LikeType != like.LikeType)
                    {
                        _like.LikeType = like.LikeType;                       
                        result = _like.likeCalculate(2);
                        _db.Likes.Update(_like);
                        await _db.SaveChangesAsync();
                    }
                    else
                        result = 0;

                    if (result != 0)
                    {
                        User author;

                        if (like.FromType == FROM_POST)
                        {
                            Post post = await _db.Posts.FindAsync(like.PostId);


                            author = await _userManager.FindByNameAsync(post.Author);


                            post.Rating += result;
                            author.Rating += result;

                            _db.Posts.Update(post);
                            await _db.SaveChangesAsync();
                        }
                        else if (like.FromType == FROM_COMMENT)
                        {
                            Comment comment = await _db.Comments.FindAsync(like.PostId);


                            author = await _userManager.FindByNameAsync(comment.Author);


                            comment.Rating += result;
                            author.Rating += result;
                            _db.Comments.Update(comment);
                            await _db.SaveChangesAsync();
                        }
                        else
                            throw new Exception("unknown post type");

                        _db.Users.Update(author);
                        await _db.SaveChangesAsync();

                        switch (result)
                        {
                            case -2: user.DisLikeCount++; user.LikeCount--; break;
                            case -1: user.DisLikeCount--; break;
                            case 1: user.LikeCount++; break;
                            case 2: user.LikeCount++; user.DisLikeCount--; break;
                        }

                        
                        _db.Users.Update(user);
                        await _db.SaveChangesAsync();
                    }
                    return Json(new { result = result });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
                }
            }

            return Json(null);
        }
        [HttpPost]
        public async Task<JsonResult> CheckLike([FromBody] LikeModel _like)
        {
            if (User.Identity.IsAuthenticated && _like != null)
            {
                const byte FROM_POST = 0;
                const byte FROM_COMMENT = 1;

                try
                {
                    User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    Like like = _like.FromType switch
                    {
                        FROM_POST => _db.Likes.FirstOrDefault(l => l.PostId == _like.PostId && l.UserId == user.Id),
                        FROM_COMMENT => _db.Likes.FirstOrDefault(l => l.CommentId == _like.PostId && l.UserId == user.Id),
                        _ => throw new Exception("Неизвестный FromType")
                    };

                    if (like != null)
                        return Json(new { result = like.LikeType });
                }
                catch(Exception ex)
                {
                    _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
                }
            }
            return Json(null);
        }
        [HttpGet]
        public async Task<IActionResult> Post(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    Post post = await _db.Posts.FindAsync(id);
                    await _db.Texts.Where(i => i.PostId == post.Id).LoadAsync();
                    await _db.Pictures.Where(i => i.PostId == post.Id).LoadAsync();
                    await _db.Comments.Where(c => c.PostId == post.Id).LoadAsync();
                    return View(post);
                }
                catch(Exception ex)
                {
                    _logger.LogWarning("Time:{0}\tPath:{1}\tExeption:{2}", DateTime.UtcNow.ToLongTimeString(), HttpContext.Request.Path, ex.Message);
                }
            }
            
            return RedirectToAction("hot");
        }
    }
}

