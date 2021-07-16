using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public string FromName { get; set; }
        public string Text { get; set; }
        public int Rainting { get; set; }
        public DateTime PostTime { get; set; }
        public int PostId{ get; set; }
    }
}
