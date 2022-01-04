using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrueStoryMVC.DataItems;
using TrueStoryMVC.DataItems.Utils;
using TrueStoryMVC.Interfaces.Repository;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Repositories
{
    public class SystemRepository : ISystemRepository
    {
        private readonly RootDb _db;
        private readonly IPostRepository _postRepository;
        public SystemRepository(
            RootDb db,
            IPostRepository postRepository)
        {
            _db = db;
            _postRepository = postRepository;
        }

        public Task<Comment> GetCommentAsync(int id) => _db.Comments.FirstOrDefaultAsync(c => c.Id == id);

        public Task<Like> GetLikeAsync(int contentId, string userId, FromLikeType type)
        {
            return type switch
            {
                FromLikeType.FROM_POST => _db.Likes.FirstOrDefaultAsync(l => l.Post.Id == contentId && l.User.Id == userId),
                FromLikeType.FROM_COMMENT => _db.Likes.FirstOrDefaultAsync(l => l.Comment.Id == contentId && l.User.Id == userId),
                _ => throw new Exception(Error.UnknownEnum(nameof(FromLikeType)))
            };
        }

        public async Task<int> LikeAsync(LikeRequest request, User user)
        {
            if (request.FromType == FromLikeType.FROM_POST)
            {
                var post = await _postRepository.GetPostAsync(request.ContentId);
                if (post == null) return 0;
                return await LikePostAsync(request, user, post);
            }

            if(request.FromType == FromLikeType.FROM_COMMENT)
            {
                var comment = await GetCommentAsync(request.ContentId);
                if (comment == null) return 0;
                return await LikeCommentAsync(request, user, comment);
            }

            throw new Exception(Error.UnknownEnum(nameof(FromLikeType)));
        }

        public LikeType CheckUserLikeType(LikeRequest request, User user)
        {
            return request.FromType switch
            {
                FromLikeType.FROM_POST => _db.Likes.FirstOrDefault(l => l.Post.Id == request.ContentId && l.User.Id == user.Id)?.LikeType ?? LikeType.NONE,
                FromLikeType.FROM_COMMENT => _db.Likes.FirstOrDefault(l => l.Comment.Id == request.ContentId && l.User.Id == user.Id)?.LikeType ?? LikeType.NONE,
                _ => throw new Exception(Error.UnknownEnum(nameof(FromLikeType)))
            };
        }

        public async Task CreateCommentAsync(CommentRequest request, User user, Post post)
        {
            var comment = new Comment
            {
                Text = request.Text,
                User = user,
                Post = post,
                PostTime = DateTime.UtcNow
            };

            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();
        }

        #region Private methods
        private async Task<int> LikePostAsync(LikeRequest request, User user, Post post)
        {
            var like = await GetLikeAsync(request.ContentId, user.Id, request.FromType);

            if (like == null)
            {
                like = new Like
                {
                    User = user,
                    Post = post
                };
                await _db.Likes.AddAsync(like);
            }

            var likeCount = like.LikeCalculate(request.LikeType);
            like.LikeType = request.LikeType;

            
            if (likeCount > 0)
            {
                like.Post.Rating += likeCount;
                like.Post.User.Rating += likeCount;

                switch (likeCount)
                {
                    case -2: user.DisLikeCount++; user.LikeCount--; break;
                    case -1: user.DisLikeCount--; break;
                    case 1: user.LikeCount++; break;
                    case 2: user.LikeCount++; user.DisLikeCount--; break;
                }
            }
            await _db.SaveChangesAsync();
            return likeCount;
        }

        private async Task<int> LikeCommentAsync(LikeRequest request, User user, Comment comment)
        {
            var like = await GetLikeAsync(request.ContentId, user.Id, request.FromType);

            if (like == null)
            {
                like = new Like
                {
                    User = user,
                    Comment = comment
                };
                await _db.Likes.AddAsync(like);
            }

            var likeCount = like.LikeCalculate(request.LikeType);
            like.LikeType = request.LikeType;
            
            if (likeCount > 0)
            {
                like.Comment.Rating += likeCount;
                like.Comment.User.Rating += likeCount;

                switch (likeCount)
                {
                    case -2: user.DisLikeCount++; user.LikeCount--; break;
                    case -1: user.DisLikeCount--; break;
                    case 1: user.LikeCount++; break;
                    case 2: user.LikeCount++; user.DisLikeCount--; break;
                }
            }
            await _db.SaveChangesAsync();
            return likeCount;
        }
        #endregion
    }
}
