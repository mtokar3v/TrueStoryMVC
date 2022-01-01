using System;
using TrueStoryMVC.Data;
namespace TrueStoryMVC.Models
{
    public class Like : ILike
    {
        private const byte NONE = 0;
        private const byte LIKE = 1;
        private const byte DISLIKE = 2;
        private byte likeType;
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }

        public byte LikeType
        {
            get
            {
                return likeType;
            }
            set
            {
                likeType = value switch
                {
                    NONE => NONE,
                    LIKE => LIKE,
                    DISLIKE => DISLIKE,
                    _ => NONE
                };
            }
        }
        public int likeCalculate(int delta)
        {
            return likeType switch
            {
                NONE => 0,
                LIKE => delta,
                DISLIKE => -delta,
                _ => throw new Exception("unknown like type")
            };

        }
    }

}
