using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TrueStoryMVC.Models
{
    public class RootDb: IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostPicture> Pictures { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Like> Likes { get; set; }

        public RootDb(DbContextOptions<RootDb> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseNpgsql("Host=localhost;Port=5432;Database=truestorydb;Username=postgres;Password=1234");

        }
    }
}
