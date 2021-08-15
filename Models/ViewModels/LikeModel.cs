using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models.ViewModels
{
    public class LikeModel
    {
        public int PostId { get; set; }
        public byte LikeType { get; set; }
    }
}
