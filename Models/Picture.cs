namespace TrueStoryMVC.Models
{
    public class PostPicture
    {
        public int Id { get; set; }
        public RectImg Picture { get; set; } = new RectImg();
        public int PostId { get; set; }
    }
}
