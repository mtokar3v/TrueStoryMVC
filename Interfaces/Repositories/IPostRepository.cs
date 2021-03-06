using DataItems;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Interfaces.Repository
{
    public interface IPostRepository
    {
        Task<Post> GetPostAsync(int id);
        Task<List<Post>> GetPostsListAsync(PostType type, int number, string arg);
        Task DeletePost(Post post);
        Task CreatePost(PostModel model, User user);
    }
}
