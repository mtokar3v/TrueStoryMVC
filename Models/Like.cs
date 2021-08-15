using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models
{
    enum LikeType : byte
    {
        NONE,
        LIKE,
        DISLIKE
    }
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public byte LikeType { get; set; } // 0:N/A; 1:LIKE; 2:DISLIKE
    }

}
