using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.ValidationAttributes
{
    public class SchemeHasRightFieldCountAttribute : ValidationAttribute
    {
        public SchemeHasRightFieldCountAttribute()
        {
            ErrorMessage = "Scheme is not valide";
        }

        public override bool IsValid(object value)
        {
            var postModel = value as PostModel;
            if (postModel == null) throw new Exception("Incorrect attibute class");

            var contentCount = 0;
            if (postModel.Texts != null) contentCount += postModel.Texts.Count();
            if (postModel.Images != null) contentCount += postModel.Images.Count();

            var isValid = SchemeHasRightFieldCount(postModel.Scheme, contentCount);
            return isValid;
        }

        private bool SchemeHasRightFieldCount(string scheme, int expectedFieldCount)
        {
            if (string.IsNullOrEmpty(scheme)) return false;

            var currentShemeFieldCount = scheme.Split(' ').Length - 1;
            return currentShemeFieldCount == expectedFieldCount;
        }
    }
}
