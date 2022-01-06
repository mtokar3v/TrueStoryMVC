using System.Collections.Generic;
using System.Linq;
using TrueStoryMVC.Builders;
using TrueStoryMVC.Models;

namespace TrueStoryMVC.Extensions
{
    public static class PostExtensions
    {
        public static List<Text> AddTexts(this Post post, List<string> texts)
        {
            if (texts == null)
            {
                post.Texts = null;
                return null;
            }

            var formatTexts = texts
                .Select(t => new Text
                {
                    Post = post,
                    TextData = t
                })
                .ToList();

            post.Texts = formatTexts;
            return formatTexts;
        }

        public static List<PostPicture> AddImages(this Post post, List<IEnumerable<byte>> images)
        {
            if (images == null)
            {
                post.Pictures = null;
                return null;
            }

            var formatImages = images
                .Select(i => new PostPicture
                    {
                        Post = post,
                        Picture = new ImageBuilder()
                            .CreateRectangleImage(i.ToArray())
                            .Build()
                    })
                .ToList();

            post.Pictures = formatImages;

            return formatImages;
        }
    }
}
