using TrueStoryMVC.Models;
using DataItems;
using System.Collections.Generic;
using System;
using TrueStoryMVC.Interfaces.Repository;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TrueStoryMVC.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly RootDb _db;

        public PostRepository(RootDb db)
        {
            _db = db;
        }

        public async Task<Post> GetPostAsync(int id) => await _db.Posts.Where(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<List<Post>> GetPostsListAsync(PostType type, int number, string arg)
        {
            var posts = type switch
            {
                PostType.HOT => await GetHotPosts(number),
                PostType.BEST => await GetBestPosts(number),
                PostType.NEW => await GetNewPosts(number),
                PostType.TAGS => GetTagsPosts(number, arg),
                PostType.USER => await GetUserPosts(number, arg),
                _=>throw new Exception("Invalid post type")
            };

            return posts;
        }

        public async Task DeletePost(Post post)
        {
            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
        }

        #region Private methods
        private int postListSize = 20;

        private Task<List<Post>> GetHotPosts(int number)
        {
            return _db.Posts
                .Where(t => t.PostTime.Day + 1 >= DateTime.UtcNow.Day && t.PostTime.Month == DateTime.UtcNow.Month && t.PostTime.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.comments.Count)
                .Skip(number * postListSize)
                .Take(postListSize)
                .ToListAsync();
        }

        private Task<List<Post>> GetBestPosts(int number)
        {
            return _db.Posts
                .Where(t => t.PostTime.Day + 1 >= DateTime.UtcNow.Day && t.PostTime.Month == DateTime.UtcNow.Month && t.PostTime.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.Rating)
                .Skip(number * postListSize)
                .Take(postListSize)
                .ToListAsync();
        }

        private Task<List<Post>> GetNewPosts(int number)
        {
            return _db.Posts
                .Where(t => t.PostTime.Day + 1 >= DateTime.UtcNow.Day && t.PostTime.Month == DateTime.UtcNow.Month && t.PostTime.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.PostTime)
                .Skip(number * postListSize)
                .Take(postListSize)
                .ToListAsync();
        }

        private List<Post> GetTagsPosts(int number, string tags)
        {
            var posts = new List<Post>();

            if (!string.IsNullOrEmpty(tags))
            {
                var tagArray = tags.Split(new char[] { ' ', ',', ';' }).ToList();

                tagArray.ForEach(async s => 
                {
                    var postWithTag = await _db.Posts
                                         .Where(p => p.Tags.Contains(s))
                                         .Skip(number * postListSize)
                                         .Take(postListSize)
                                         .Where(p => !posts.Contains(p))
                                         .Select(p => p)
                                         .ToListAsync();

                    posts.AddRange(postWithTag);
                });
            }
            return posts;
        }

        private Task<List<Post>> GetUserPosts(int number, string user)
        {
            return _db.Posts 
                .Where(p => p.User.UserName == user)
                .OrderByDescending(p => p.Rating)
                .Skip(number * postListSize)
                .Take(postListSize)
                .ToListAsync();
        }

        
        #endregion
    }
}
