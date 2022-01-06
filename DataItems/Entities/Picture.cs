namespace TrueStoryMVC.Models
{
    public class PostPicture
    {
        public int Id { get; set; }
        public Img Picture { get; set; }
        public virtual Post Post { get; set; }
    }
}
