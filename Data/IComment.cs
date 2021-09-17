using System;

namespace TrueStoryMVC.Data
{
    interface IComment : IDataInfo
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
    }
}
