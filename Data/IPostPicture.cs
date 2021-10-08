using TrueStoryMVC.Models;

namespace TrueStoryMVC.Data
{
    interface IPostPicture
    {
        public int Id { get; set; }
        public Img Picture { get; set; }
        public int PostId { get; set; }
    }
}
