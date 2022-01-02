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
        public virtual List<User> Subscribers { get; set; } = new List<User>();
        public virtual List<Like> Likes { get; set; } = new List<Like>();

        public User()
        {
            RegisterTime = DateTime.UtcNow;
        }
    }
}
