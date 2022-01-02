﻿using System.ComponentModel.DataAnnotations;
using TrueStoryMVC.DataItems;

namespace TrueStoryMVC.Models.ViewModels
{
    public class LikeRequest
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public LikeType LikeType { get; set; }

        [Required]
        public FromLikeType FromType { get; set; } 
    }
}
