using System.Security.Claims;
using System.Threading.Tasks;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(ClaimsPrincipal User);
        Task<User> GetUserAsync(string userName);
        Task UpdateAvatar(ChangeAvatarRequest request, User user);
        bool HasAccessToDelete(ClaimsPrincipal principal);

    }
}
