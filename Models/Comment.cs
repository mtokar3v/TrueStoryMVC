using System;
using TrueStoryMVC.Data;

namespace TrueStoryMVC.Models
{
    public class Comment : IComment
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime PostTime { get; set; }
        public int PostId { get; set; }
        public virtual User User { get; set; }
    }
}
