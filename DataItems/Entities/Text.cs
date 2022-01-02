namespace TrueStoryMVC.Models
{
    public class Text
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string TextData { get; set; }
        public virtual User User { get; set; }
    }
}
