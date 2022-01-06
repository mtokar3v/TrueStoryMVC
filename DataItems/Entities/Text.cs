namespace TrueStoryMVC.Models
{
    public class Text
    {
        public int Id { get; set; }
        public string TextData { get; set; }
        public virtual Post Post { get; set; }
    }
}
