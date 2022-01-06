using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrueStoryMVC.ValidationAttributes;

namespace TrueStoryMVC.Models.ViewModels
{
    [PostModelHasContent]
    [SchemeHasRightFieldCount]
    public class PostModel
    {
        [Required]
        public string Header { get; set; }

        public List<string> Texts { get; set; }

        public List<IEnumerable<byte>> Images { get; set; }

        [Required]
        public string TagsLine { get; set; }

        [Required]
        public string Scheme { get; set; }
    }
}
