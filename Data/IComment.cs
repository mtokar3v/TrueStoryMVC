using System;

namespace TrueStoryMVC.Data
{
    interface IComment
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public string FromName { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime PostTime { get; set; }
        public int PostId { get; set; }
    }
}
