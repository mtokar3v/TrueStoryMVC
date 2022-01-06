using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrueStoryMVC.Models.ViewModels
{
    public class ChangeAvatarRequest
    {
        [Required]
        public IEnumerable<byte> Data { get; set; }
    }
}
