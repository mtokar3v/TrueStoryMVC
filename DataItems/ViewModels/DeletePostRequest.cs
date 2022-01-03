using System.ComponentModel.DataAnnotations;

namespace TrueStoryMVC.DataItems.ViewModels
{
    public class DeletePostRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
