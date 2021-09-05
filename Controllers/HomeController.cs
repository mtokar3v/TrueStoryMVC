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

        public async Task<IActionResult> CreatePost([FromBody] PostModel postModel)
        {
            Console.WriteLine(postModel.Header);
            Console.WriteLine(postModel.TagsLine);

            foreach (var i in postModel.Texts)
                Console.WriteLine(i);

            foreach (var i in postModel.Scheme)
                Console.WriteLine(i);

            return Ok();
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> Add(PostModel pvm)
        //{
        //    Post post = new Post { Header = pvm.Header, PostTime = DateTime.Now.ToUniversalTime(), Author = User.Identity.Name };
        //    if (!String.IsNullOrEmpty(pvm.Text)) post.Text = pvm.Text;
        //    db.Posts.Add(post);
        //    await db.SaveChangesAsync();

        //    if (pvm.Image != null)
        //    {
        //        foreach (IFormFile formFile in pvm.Image)
        //        {
        //            //мне эта часть не нравится:
        //            // 1) приходится открывать два потока, с одним не работает, так как b.save() плохо работает и не перезаписывает байты (по моему мнению)
        //            // 2) чтобы считать с формы, я сначала из возвращаемого потока stream(метод openreadstream) создаю массив байт, после чего из него создаю поток memorystream
        //            byte[] bytes = new byte[formFile.Length];
        //            formFile.OpenReadStream().Read(bytes, 0, (int)formFile.Length);
        //            using (var reader = new MemoryStream(bytes))
        //            {
        //                using (Image image = Image.FromStream(reader))
        //                {

        //                    int w = image.Width;
        //                    int h = image.Height;

        //                    double k = (double)h / w;
        //                    w = 600;
        //                    h = (int)(w * k);

        //                    using (Bitmap b = new Bitmap(image, w, h))
        //                    {
        //                        using (var reader2 = new MemoryStream())
        //                        {
        //                            b.Save(reader2, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                            bytes = new byte[reader2.ToArray().Length];
        //                            bytes = reader2.ToArray();
        //                        }
        //                    }

        //                    ImageInfo img = new ImageInfo()
        //                    {
        //                        Data = bytes,
        //                        Width = w,
        //                        Height = h,
        //                        PostId = post.Id
        //                    };

        //                    db.Images.Add(img);
        //                }
        //            }
        //        }
        //    }

        //    if (!String.IsNullOrEmpty(pvm.TagsLine))
        //    {
        //        post.Tags = pvm.TagsLine;
        //    }
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Hot");
        //}

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
        public async Task<IActionResult> Tag(string SomeTags)
        {
            List<Post> posts = null;
            if (!String.IsNullOrEmpty(SomeTags))
            {
                string[] tagArray = SomeTags.Split(new char[] { ' ', ',', ';' });
                foreach (string s in tagArray)
                    posts = await db.Posts.Where(p => p.Tags.Contains(s)).ToListAsync();

                foreach (var p in posts)
                    await db.Images.Where(i => i.PostId == p.Id).LoadAsync();
            }
            return View(posts);
        }

        public async Task<IActionResult> GetPostBlock([FromBody]PostBlockInfo postBlock)
        {
            const byte HOT = 0;
            const byte BEST = 1;
            const byte NEW = 2;

            DateTime time = DateTime.UtcNow;

            IQueryable<Post> p_query = postBlock.PostBlockType switch
            {
                HOT => db.Posts.Where(t => t.PostTime.Day + 1 > time.Day && t.PostTime.Month == time.Month && t.PostTime.Year == time.Year).OrderByDescending(p => p.comments.Count).Skip(postBlock.Number * 2).Take(2),
                BEST => db.Posts.Where(t => t.PostTime.Day + 1 > time.Day && t.PostTime.Month == time.Month && t.PostTime.Year == time.Year).OrderByDescending(p => p.Rating).Skip(postBlock.Number * 2).Take(2),
                NEW => db.Posts.Where(t => t.PostTime.Day + 1 > time.Day && t.PostTime.Month == time.Month && t.PostTime.Year == time.Year).OrderByDescending(p => p.PostTime).Skip(postBlock.Number * 2).Take(2),
                _=>throw new Exception("Неизвестный PostBlockType")
            };

            var posts = await p_query.ToListAsync();

            foreach(var item in posts)
                await db.Images.Where(i => i.PostId == item.Id).LoadAsync();

            return PartialView("_GetPosts", posts);
        }

        [HttpPost]
        public async Task<IActionResult> Comment([FromForm] CommentModel comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                Comment newComment = new Comment { Text = comment.Text, FromName = user.UserName, PostTime = DateTime.Now.ToUniversalTime(), PostId = comment.PostId };
                db.Comments.Add(newComment);
                await db.SaveChangesAsync();
                return Ok();
            }
            else 
                return Unauthorized();
        }

        public class Person
        {
            public int Count { get; set; }
        }

        [HttpPost]
        public async Task<JsonResult> Like([FromBody] LikeModel like)
        {
            Console.WriteLine(like.PostId);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                const byte FROM_POST = 0;
                const byte FROM_COMMENT = 1;

                int result;
                //cache!!!!!!!!!!!
                User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                Like _like;
                if (like.FromType == FROM_POST)
                   _like = await db.Likes.FirstOrDefaultAsync(l => l.PostId == like.PostId && l.UserId == user.Id);
                else if (like.FromType == FROM_COMMENT)
                    _like = await db.Likes.FirstOrDefaultAsync(l => l.CommentId == like.PostId && l.UserId == user.Id);
                else
                    return Json(null);

                if (_like == null)
                {
                    _like = new Like { LikeType = like.LikeType, UserId = user.Id };
                    if (like.FromType == FROM_POST)
                        _like.PostId = like.PostId;
                    else if(like.FromType == FROM_COMMENT)
                        _like.CommentId = like.PostId;

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

                    User author;
                    
                    //не помешал бы класс который все это делает обобщенно

                    if (like.FromType == FROM_POST)
                    {
                        Post post = await db.Posts.FindAsync(like.PostId);
                        author = await _userManager.FindByNameAsync(post.Author);
                        post.Rating += result;
                        author.Rating += result;
                        db.Posts.Update(post);
                    }
                    else if (like.FromType == FROM_COMMENT)
                    {
                        Comment comment = await db.Comments.FindAsync(like.PostId);
                        author = await _userManager.FindByNameAsync(comment.FromName);
                        comment.Rating += result;
                        author.Rating += result;
                        db.Comments.Update(comment);
                    }
                    
                     
                    
                    await db.SaveChangesAsync();
                }
                return Json(new { result = result });
            }
            else
                return Json(null);
        }



        [NonAction]
        private int likeCalculate(byte likeType, int delta)
        {
            const byte NONE = 0;
            const byte LIKE = 1;
            const byte DISLIKE = 2;

            return likeType switch
            {
                NONE => 0,
                LIKE => delta,
                DISLIKE => -delta,
                _ => throw new Exception("Неивестный LikeType")
            };

        }

        //В модели лишь одно свойство, нужно подумать
        public async Task<JsonResult> CheckLike([FromBody]LikeModel _like)
        {
            if (User.Identity.IsAuthenticated)
            {
                const byte FROM_POST = 0;
                const byte FROM_COMMENT = 1;

                //в like должен быть username, а не id. Либо id должен быть int, а не string
                User user = await _userManager.FindByNameAsync(User.Identity.Name);

                Like like = _like.FromType switch
                {
                    FROM_POST => db.Likes.FirstOrDefault(l => l.PostId == _like.PostId && l.UserId == user.Id),
                    FROM_COMMENT => db.Likes.FirstOrDefault(l => l.CommentId == _like.PostId && l.UserId == user.Id),
                    _=> throw new Exception("Неизвестный FromType")
                };

                if (like != null)  
                    return Json(new { result = like.LikeType });
            }
            return Json(null);
        }

        public async Task<IActionResult> Post(int? id)
        {
            if (id != null)
            {
                Post post = await db.Posts.FindAsync(id);
                await db.Images.Where(i => i.PostId == post.Id).LoadAsync();
                await db.Comments.Where(c => c.PostId == post.Id).LoadAsync();
                return View(post);
            }
            else
                return RedirectToAction("hot");
        }
    }
}

