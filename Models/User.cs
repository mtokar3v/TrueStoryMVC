using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TrueStoryMVC.Models
{
    public class User: IdentityUser
    {
        private readonly DateTime registerTime;
        public int Rating { get; set; }
        public int CommentCount { get; set; }
        public int PostCount { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime RegisterTime { get; }
        public List<User> Subscribers { get; set;}
        public List<Like> Likes { get; set; }

        public User()
        {
            registerTime = DateTime.UtcNow;
        }
    }
}
