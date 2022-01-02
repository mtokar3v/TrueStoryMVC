using System.ComponentModel.DataAnnotations;

namespace TrueStoryMVC.Models.ViewModels
{
    public class CommentRequest
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public byte CommentType { get; set; }
    }
}
