using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Drawing;   
using System.Web;

namespace TrueStoryMVC.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PostModel pvm)
        {
            Post post = new Post{Header = pvm.Header};
            if (!String.IsNullOrEmpty(pvm.Text)) post.Text = pvm.Text;
            db.Posts.Add(post);
            await db.SaveChangesAsync();

            if (pvm.Image != null)
            {
                foreach (IFormFile formFile in pvm.Image)
                {
                    byte[] bytes = new byte[formFile.Length];
                    formFile.OpenReadStream().Read(bytes, 0, (int)formFile.Length);
                    using (var reader = new MemoryStream(bytes))
                    {
                        using (Image image = Image.FromStream(reader))
                        {

                            int w = image.Width;
                            int h = image.Height;

                           // int k = h / w;
                            w /=200;
                            h /=200;

                            using (Bitmap b = new Bitmap(image, w, h))
                            {
                                /*значит, дело такое, поток нужно почистить или создать заново*/
                                var reader2 = new MemoryStream();
                                b.Save(reader2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                bytes = new byte[reader2.ToArray().Length];
                                bytes = reader2.ToArray();
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
            await db.SaveChangesAsync();

            Post post1 = db.Posts.First();
            return RedirectToAction("Hot");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id!=null)
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
            List<ImageInfo> images = db.Images.ToList();
            return View(await db.Posts.ToListAsync());
        }

        public async Task<IActionResult> Img()
        {
            return View(await db.Images.ToListAsync());
        }
    }
}
