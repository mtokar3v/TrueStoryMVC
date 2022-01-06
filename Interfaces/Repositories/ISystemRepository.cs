using System.Threading.Tasks;
using TrueStoryMVC.DataItems;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Interfaces.Repository
{
    public interface ISystemRepository
    {
        Task<int> LikeAsync(LikeRequest request, User user);
        Task CreateCommentAsync(CommentRequest request, User user, Post post);
        LikeType CheckUserLikeType(LikeRequest request, User user);
    }
}
