using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.ValidationAttributes
{
    public class PostModelHasContentAttribute : ValidationAttribute
    {
        public PostModelHasContentAttribute()
        {
            ErrorMessage = "Post model does not has a content";
        }

        public override bool IsValid(object value)
        {
            var postModel = value as PostModel;
            if (postModel == null) throw new Exception("Incorrect attibute class");

            var isValid = false;

            isValid |= AnyImageHaveValue(postModel.Images);
            isValid |= AnyTextsHaveValue(postModel.Texts);

            return isValid;
        }

        private bool AnyTextsHaveValue(IEnumerable<string> texts)
        {
            if (texts == null) return false;
            return texts.Any(t => !string.IsNullOrEmpty(t));
        }

        private bool AnyImageHaveValue(List<IEnumerable<byte>> images)
        {
            if (images == null) return false;
            return images.Any(i => i.Any());
        }
    }
}
