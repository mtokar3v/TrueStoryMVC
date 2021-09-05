using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models
{
    public class Text
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string TextData { get; set; }
    }
}
