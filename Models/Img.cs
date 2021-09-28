using Microsoft.EntityFrameworkCore;
using TrueStoryMVC.Data;

namespace TrueStoryMVC.Models
{
    [Owned]
    public class Img : IImage
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public byte[] Data { get; set; }
    }
}
