using DataItems;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueStoryMVC.Models;

namespace WebApiLib.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPostsListAsync(PostTypes type, int number, string arg);
    }
}
