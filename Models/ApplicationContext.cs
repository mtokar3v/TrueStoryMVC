using Microsoft.EntityFrameworkCore;

namespace TrueStoryMVC.Models
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
