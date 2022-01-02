using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using TrueStoryMVC.Interfaces.Repository;
using TrueStoryMVC.Models;

namespace TrueStoryMVC.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal User) => await _userManager.GetUserAsync(User);
    }
}
