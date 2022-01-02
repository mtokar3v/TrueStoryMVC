using System;
using TrueStoryMVC.DataItems;

namespace TrueStoryMVC.Models
{
    public class Like
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }
        public LikeType LikeType { get; set; }

        public int LikeCalculate(LikeType newType)
        {
            if(LikeType == newType) return 0;

            return LikeType switch
            {
                LikeType.NONE => calc(newType, 1),
                _ => calc(newType, 2)
            };
        }

        private int calc(LikeType newType, int offset)
        {
            return newType switch
            {
                LikeType.LIKE => offset,
                LikeType.DISLIKE => -offset,
                _=> throw new Exception("Unknown likeType")
            };
        }
    }
}