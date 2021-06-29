using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models.ViewModels
{
    public class PostModel
    {
        public string Header { get; set; }
        public string Text { get; set; }
        public IFormFile Image { get; set; }
    }
}
