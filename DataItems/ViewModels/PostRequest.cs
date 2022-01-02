using DataItems;
using System.ComponentModel.DataAnnotations;

namespace TrueStoryMVC.Models.ViewModels
{
    public class PostRequest
    {
        [Required]
        public PostType PostBlockType { get; set; }

        [Required]
        public int Number { get; set; }

        public string Argument { get; set; }
    }
}
