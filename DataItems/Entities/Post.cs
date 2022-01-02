using System;
using System.Collections.Generic;

namespace TrueStoryMVC.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public int Rating { get; set; }
        public DateTime PostTime { get; set; }
        public string Tags { get; set; }
        public virtual User User { get; set; }
        public virtual List<Text> Texts { get; set; } = new List<Text>();
        public virtual List<Comment> comments { get; set; } = new List<Comment>();
        public virtual List<PostPicture> Pictures { get; set; } = new List<PostPicture>();
        public string Scheme { get; set; }
    }
}
