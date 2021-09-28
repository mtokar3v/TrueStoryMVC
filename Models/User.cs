using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TrueStoryMVC.Models
{
    public class User: IdentityUser
    {
        public int Rating { get; set; }
        public int CommentCount { get; set; }
        public int PostCount { get; set; }
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public Img Picture { get; set; }
        public DateTime RegisterTime { get; set; }
        public List<User> Subscribers { get; set;}
        public List<Like> Likes { get; set; }

        public User()
        {
            RegisterTime = DateTime.UtcNow;
        }
    }
}
