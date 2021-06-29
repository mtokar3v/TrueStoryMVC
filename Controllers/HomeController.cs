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
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(pvm.Image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)pvm.Image.Length);
                }
                post.Image = imageData;
            }
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Hot");
        }
        public async Task<IActionResult> Hot()
        {
            return View(await db.Posts.ToListAsync());
        }

        public IActionResult Best()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult Subs()
        {
            return View();
        }

        public IActionResult Communities()
        {
            return View();
        }

    }
}
