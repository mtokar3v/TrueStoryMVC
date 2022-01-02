using System;

namespace TrueStoryMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime PostTime { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public string Author() => User.UserName;
    }
}
