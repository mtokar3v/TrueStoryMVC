using System;
using System.Collections.Generic;
using TrueStoryMVC.Models;

namespace TrueStoryMVC.Data
{
    interface IPost
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public int Rating { get; set; }
        public string Author { get; set; }
        public DateTime PostTime { get; set; }
        public string Tags { get; set; }
        public List<Text> Texts { get; set; }
        public List<Comment> comments { get; set; } 
        public List<Picture> Pictures { get; set; }
        public string Scheme { get; set; }
    }
}
