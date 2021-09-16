using TrueStoryMVC.Data;
namespace TrueStoryMVC.Models
{
    public class Like : ILike
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public byte LikeType { get; set; } // 0:N/A; 1:LIKE; 2:DISLIKE
    }

}
