namespace TrueStoryMVC.Models
{
    public class PostPicture
    {
        public int Id { get; set; }
        public Img Picture { get; set; }
        public int PostId { get; set; }
        public virtual User User { get; set; }
    }
}
