using Microsoft.EntityFrameworkCore;

namespace TrueStoryMVC.Models
{
    [Owned]
    public class Img
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public byte[] Data { get; set; }
    }
}
