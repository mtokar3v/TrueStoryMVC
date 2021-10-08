using System;
using System.Collections.Generic;
using TrueStoryMVC.Data;

namespace TrueStoryMVC.Models
{
    public class Post : IPost
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public int Rating { get; set; }
        public string Author { get; set; }
        public DateTime PostTime { get; set; }
        public string Tags { get; set; }
        public List<Text> Texts { get; set; } = new List<Text>();
        public List<Comment> comments { get; set; } = new List<Comment>();
        public List<PostPicture> Pictures { get; set; } = new List<PostPicture>();
        public string Scheme { get; set; }
    }
}
