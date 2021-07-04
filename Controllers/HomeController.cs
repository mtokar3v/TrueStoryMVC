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
            Post post = new Post
            {
                Header = pvm.Header,  
            };

            if (!String.IsNullOrEmpty(pvm.Text))
                post.Text = pvm.Text;

            if (pvm.Image != null)
            {
                foreach (IFormFile formFile in pvm.Image)
                {
                    byte[] bytes = new byte[formFile.Length];

                    using (Stream reader = formFile.OpenReadStream())
                    {
                        //если что эта часть нужна для дальнейшей работы с изображением (изменение размера, удаления метаданных и тд)
                        using (Image image = Image.FromStream(reader))
                        {

                            int w = image.Width;
                            int h = image.Height;

                            using (Bitmap b = new Bitmap(image, new Size(w, h)))
                            {
                                b.Save(reader, System.Drawing.Imaging.ImageFormat.Jpeg);
                                await reader.ReadAsync(bytes,0, (int)formFile.Length);
                            }

                            ImageInfo img = new ImageInfo()
                            {
                                Data = bytes,
                                Width = w,
                                Height = h       
                            };
                            
                            db.Images.Add(img);
                            post.PostImages.Add(img);
                            
                        }
                    }
                }
            }
            //на этом этапе все правильно добавляется (как в бд, так и в модель)
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Hot");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id!=null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(p => p.PostId == id);
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
            List<Post> list = db.Posts.ToList();
            foreach(var i in list)
            {
                foreach(var t in i.PostImages)
                {
                    
                }
            }
            return View(await db.Posts.ToListAsync());
        }

        public async Task<IActionResult> Img()
        {
            return View(await db.Images.ToListAsync());
        }
    }
}
