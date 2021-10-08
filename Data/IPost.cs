using System;
using System.Collections.Generic;
using TrueStoryMVC.Models;

namespace TrueStoryMVC.Data
{
    interface IPost : IDataInfo
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Tags { get; set; }
        public List<Text> Texts { get; set; }
        public List<Comment> comments { get; set; }
        public List<PostPicture> Pictures { get; set; }
        public string Scheme { get; set; }
    }
}
