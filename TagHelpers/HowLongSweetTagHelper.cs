using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace TrueStoryMVC.TagHelpers
{
    public class HowLongSweetTagHelper : TagHelper
    {
        public DateTime PostTime { get; set; } = DateTime.Now;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;

            DateTime time = DateTime.Now.ToUniversalTime();
            TimeSpan delta = time.Subtract(PostTime);

            string result = string.Empty;
            string article;

            if ((int)delta.TotalDays / 365 > 0)
            {
                int y = (int)delta.TotalDays / 365;
                if (y == 1)
                    article = "год";
                else
                if (y < 5)
                    article = "года";
                else
                    article = "лет";

                result = string.Concat(result, y, ' ' ,article, ' ');
            }
            if ((int)(delta.TotalDays / 12) % 12 > 0)
            {
                int m = (int)(delta.TotalDays / 12) % 12;
                if (m == 1)
                    article = "месяц";
                else
                    if (m < 5)
                    article = "месяца";
                else
                    article = "месяцев";
                result = string.Concat(result, m, ' ' ,article, ' ');
            }
            if ((int)(delta.TotalDays / 7) % 4 > 0)
            {
                int w = (int)(delta.TotalDays / 7) % 4;
                if (w == 1)
                    article = "неделя";
                else
                    if (w < 5)
                    article = "недели";
                else
                    article = "неедель";
                result = string.Concat(result, w, ' ' ,article, ' ');
            }
            if ((int)delta.TotalDays % 7 >= 0)
            {
                int d = (int)delta.TotalDays % 7;
                if (d == 1 || d == 21 || d == 31)
                    article = "день";
                else
                    if (d < 5 && d > 0 || d > 21 && d < 25)
                    article = "дня";
                else
                    article = "дней";
                result = string.Concat(result, d, ' ' ,article, ' ');
            }

            output.Content.SetContent(result);
        }
    }
}
