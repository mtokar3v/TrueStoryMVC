using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrueStoryMVC.Builders;
using TrueStoryMVC.Extensions;
using TrueStoryMVC.Interfaces.Repository;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal user) => await _userManager.GetUserAsync(user);
        public async Task<User> GetUserAsync(string name) => await _userManager.FindByNameAsync(name);
        public async Task<User> GetUserByIdAsync(string id) => await _userManager.FindByIdAsync(id);
        public Task UpdateAvatar(ChangeAvatarRequest request, User user)
        {
            user.Picture = new ImageBuilder()
                        .CreateSquareImage(request.Data.ToArray())
                        .Build();

            return _userManager.UpdateAsync(user);
        }
        public bool HasAccessToDelete(ClaimsPrincipal principal) => principal.AtLeastAdmin();
    }
}
