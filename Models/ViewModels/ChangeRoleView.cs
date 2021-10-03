using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models.ViewModels
{
    public class ChangeRoleView
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
