using TrueStoryMVC.Models;
using DataItems;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using WebApiLib.Interfaces.Repositories;

namespace WebApiLib.Repositories
{
    public class PostRepository : IPostRepository
    {
        private RootDb _db;

        public PostRepository(RootDb db)
        {
            _db = db;
        }

        public async Task<List<Post>> GetPostsListAsync(PostTypes type, int number, string arg)
        {
            var posts = type switch
            {
                PostTypes.HOT => GetHotPosts(number),
                PostTypes.BEST => GetBestPosts(number),
                PostTypes.NEW => GetNewPosts(number),
                PostTypes.TAGS => GetTagsPosts(number, arg),
                PostTypes.USER => GetUserPosts(number, arg),
                _=>throw new Exception("Invalid post type")
            };

            //foreach (var item in posts)
            //{
            //    var l = _db.Texts.Where(i => i.PostId == item.Id).Select(t=>t).ToList();
            //    await _db.Pictures.Where(i => i.PostId == item.Id).LoadAsync();
            //}

            return posts.ToList();
        }

        #region Private methods
        private int postListSize = 20;

        private IQueryable<Post> GetHotPosts(int number)
        {
            return _db.Posts
                .Where(t => t.PostTime.Day + 1 >= DateTime.UtcNow.Day && t.PostTime.Month == DateTime.UtcNow.Month && t.PostTime.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.comments.Count)
                .Skip(number * postListSize)
                .Take(postListSize);
        }

        private IQueryable<Post> GetBestPosts(int number)
        {
            return _db.Posts
                .Where(t => t.PostTime.Day + 1 >= DateTime.UtcNow.Day && t.PostTime.Month == DateTime.UtcNow.Month && t.PostTime.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.Rating)
                .Skip(number * postListSize)
                .Take(postListSize);
        }

        private IQueryable<Post> GetNewPosts(int number)
        {
            return _db.Posts
                .Where(t => t.PostTime.Day + 1 >= DateTime.UtcNow.Day && t.PostTime.Month == DateTime.UtcNow.Month && t.PostTime.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.PostTime)
                .Skip(number * postListSize)
                .Take(postListSize);
        }

        //в post попадает только последний тег
        private IQueryable<Post> GetTagsPosts(int number, string tags)
        {
            IQueryable<Post> posts = null;

            if (!string.IsNullOrEmpty(tags))
            {
                string[] tagArray = tags.Split(new char[] { ' ', ',', ';' });
                foreach (string s in tagArray)
                {
                    posts = _db.Posts
                        .Where(p => p.Tags.Contains(s))
                        .Skip(number * postListSize)
                        .Take(postListSize);
                }
            }
            return posts;
        }

        private IQueryable<Post> GetUserPosts(int number, string user)
        {
            return _db.Posts 
                .Where(p => p.Author == user)
                .OrderByDescending(p => p.Rating)
                .Skip(number * postListSize)
                .Take(postListSize);
        }

        
        #endregion
    }
}
