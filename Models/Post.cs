using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public int Raiting { get; set; }
        public DateTime PostTime { get; set; }
        public string Tags { get; set; }
        public List<Comment> comments { get; set; } = new List<Comment>();
        public List<ImageInfo> PostImages { get; set; } = new List<ImageInfo>();
    }
}
