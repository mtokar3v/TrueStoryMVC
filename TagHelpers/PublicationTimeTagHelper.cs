using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace TrueStoryMVC.TagHelpers
{
    public class PTimeTagHelper : TagHelper
    {
        public DateTime PostTime { get; set; } = DateTime.Now;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            DateTime time = DateTime.Now.ToUniversalTime();
            TimeSpan delta = time.Subtract(PostTime);
            string result = "";
            if (delta.Minutes < 1)
                result = delta.Seconds.ToString() + "сек. назад";
            else if (delta.Hours < 1)
                result = delta.Minutes.ToString() + "мин. назад";
            else if (delta.Days < 1)
                result = delta.Hours.ToString() + "ч. назад";
            else if (delta.Days <= 7)
                result = delta.Days.ToString() + "д. назад";
            else if (delta.Days > 7)
                result = time.ToShortDateString();

            output.Content.SetContent(result);
        }
    }
}
