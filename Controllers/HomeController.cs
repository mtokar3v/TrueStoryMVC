using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Drawing;   
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace TrueStoryMVC.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        private readonly UserManager<User> _userManager;

        public HomeController(ApplicationContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            db = context;
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(PostModel pvm)
        {
            Post post = new Post { Header = pvm.Header, PostTime = DateTime.Now.ToUniversalTime(), Author = User.Identity.Name };
            if (!String.IsNullOrEmpty(pvm.Text)) post.Text = pvm.Text;
            db.Posts.Add(post);
            await db.SaveChangesAsync();

            if (pvm.Image != null)
            {
                foreach (IFormFile formFile in pvm.Image)
                {
                    //мне эта часть не нравится:
                    // 1) приходится открывать два потока, с одним не работает, так как b.save() плохо работает и не перезаписывает байты (по моему мнению)
                    // 2) чтобы считать с формы, я сначала из возвращаемого потока stream(метод openreadstream) создаю массив байт, после чего из него создаю поток memorystream
                    byte[] bytes = new byte[formFile.Length];
                    formFile.OpenReadStream().Read(bytes, 0, (int)formFile.Length);
                    using (var reader = new MemoryStream(bytes))
                    {
                        using (Image image = Image.FromStream(reader))
                        {

                            int w = image.Width;
                            int h = image.Height;

                            double k = (double)h / w;
                            w = 600;
                            h = (int)(w * k);

                            using (Bitmap b = new Bitmap(image, w, h))
                            {
                                using (var reader2 = new MemoryStream())
                                {
                                    b.Save(reader2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    bytes = new byte[reader2.ToArray().Length];
                                    bytes = reader2.ToArray();
                                }
                            }

                            ImageInfo img = new ImageInfo()
                            {
                                Data = bytes,
                                Width = w,
                                Height = h,
                                PostId = post.Id
                            };

                            db.Images.Add(img);
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(pvm.TagsLine))
            {
                post.Tags = pvm.TagsLine;
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Hot");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
                if (post != null)
                {
                    db.Posts.Remove(post);
                    await db.SaveChangesAsync();
                }
                else
                    return NotFound();
            }
            return RedirectToAction("Hot");
        }

        public async Task<IActionResult> Hot()
        {
            //нужен include по признаку
            List<Comment> comments = db.Comments.ToList();
            List<ImageInfo> images = db.Images.ToList();
            return View(await db.Posts.ToListAsync());
        }
        public IActionResult Tag(string SomeTags)
        {
            IQueryable posts = null;
            if (!String.IsNullOrEmpty(SomeTags))
            {
                //нужен include по признаку
                List<ImageInfo> images = db.Images.ToList();
                string[] tagArray = SomeTags.Split(new char[] { ' ', ',', ';' });
                foreach (string s in tagArray)
                    posts = db.Posts.Where(p => p.Tags.Contains(s));
            }
            return View(posts);
        }

        //public async Task<IActionResult> Comment(CommentModel comment)
        //{
        //    User user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    Comment newComment = new Comment { PostId = comment.PostId, Text = comment.Text, FromName = user.UserName, PostTime = DateTime.Now.ToUniversalTime() };
        //    db.Comments.Add(newComment);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("hot");
        //}

        [HttpPost]
        public async Task<IActionResult> Comment([FromForm] CommentModel comment)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Comment newComment = new Comment { PostId = comment.PostId, Text = comment.Text, FromName = user.UserName, PostTime = DateTime.Now.ToUniversalTime() };
            db.Comments.Add(newComment);
            await db.SaveChangesAsync();
            return Ok();
        }

        public class Person
        {
            public int Count { get; set; }
        }

        [HttpPost]
        public async Task<JsonResult> Like([FromBody] LikeModel like)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int result;
                //cache!!!!!!!!!!!
                User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                Like _like = await db.Likes.FirstOrDefaultAsync(l => l.PostId == like.PostId && l.UserId == user.Id);

                if (_like == null)
                {
                    _like = new Like { PostId = like.PostId, LikeType = like.LikeType, UserId = user.Id };
                    db.Likes.Add(_like);
                    await db.SaveChangesAsync();
                    result = likeCalculate(like.LikeType, 1);
                }
                else if (_like.LikeType != like.LikeType)
                {
                    _like.LikeType = like.LikeType;
                    db.Likes.Update(_like);
                    await db.SaveChangesAsync();
                    result = likeCalculate(like.LikeType, 2);
                }
                else
                    result = 0;

                if (result != 0)
                {
                    Post post = await db.Posts.FindAsync(like.PostId);
                    User author = await _userManager.FindByNameAsync(post.Author);
                    author.Rating += result;
                    post.Rating += result;
                    db.Posts.Update(post);
                    await db.SaveChangesAsync();
                }
                return Json(new { result = result });
            }
            else
                return Json(null);
        }

        [NonAction]
        int likeCalculate(byte likeType, int delta)
        {
            return likeType switch
            {
                (byte)LikeType.NONE => 0,
                (byte)LikeType.LIKE => delta,
                (byte)LikeType.DISLIKE => -delta,
                _ => throw new Exception("Неивестный LikeType")
            };

        }
    }
}
