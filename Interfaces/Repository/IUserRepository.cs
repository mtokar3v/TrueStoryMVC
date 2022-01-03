using System.Security.Claims;
using System.Threading.Tasks;
using TrueStoryMVC.Models;

namespace TrueStoryMVC.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(ClaimsPrincipal User);
        bool HasAccessToDelete(ClaimsPrincipal principal);
    }
}
