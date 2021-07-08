using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TrueStoryMVC.Models
{
    [Index(nameof(UserName))]
    public class User: IdentityUser 
    {
        public int Raiting { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public List<User> Subscribers { get; set;}
    }
}
