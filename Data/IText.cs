namespace TrueStoryMVC.Data
{
    interface IText
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string TextData { get; set; }
    }
}
