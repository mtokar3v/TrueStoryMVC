using System.Threading.Tasks;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Interfaces.Repository
{
    public interface ISystemRepository
    {
        Task<int> LikeAsync(LikeRequest request, User user);
        Task CreateCommentAsync(CommentRequest request, User user, Post post);
    }
}
