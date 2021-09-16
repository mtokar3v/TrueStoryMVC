using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TrueStoryMVC.Models
{
    public class User: IdentityUser 
    {
        public int Rating { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public List<User> Subscribers { get; set;}
        public List<Like> Likes { get; set; }
    }
}
