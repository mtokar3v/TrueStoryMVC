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
using TrueStoryMVC.DataItems;

namespace TrueStoryMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly RootDb _db;
        private readonly IMemoryCache _cache;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        public HomeController(
            RootDb dbContext,
            UserManager<User> userManager,
            ILogger<HomeController> logger,
            IMemoryCache cache)
        {
            _logger = logger;
            _userManager = userManager;
            _db = dbContext;
            _cache = cache;
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

                        Post post = new Post { Header = postModel.Header, PostTime = DateTime.Now.ToUniversalTime(), User = user, Tags = postModel.TagsLine, Scheme = postModel.Scheme };
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
                    await _db.Comments.Where(c => c.Post.Id == post.Id).LoadAsync();
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

