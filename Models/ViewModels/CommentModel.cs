using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models.ViewModels
{
    public class CommentModel
    {
        public int PostId { get; set; }
        public string Text { get; set; }
        public byte CommentType { get; set; }
    }
}
