using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Data
{
    interface ILike
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public byte LikeType { get; set; }
    }
}
