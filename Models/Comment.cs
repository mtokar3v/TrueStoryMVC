using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models
{
    enum CommentType : byte
    {
        FROM_POST,
        FROM_COMMENT
    }
    public class Comment
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
