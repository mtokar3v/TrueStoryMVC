using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TrueStoryMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace TrueStoryMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            services.AddIdentity<User, IdentityRole>(opts => {
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters =
            "àáâãäå¸æçèéêëìíîïğñòóôõö÷øùúûüışÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏĞÑÒÓÔÕÖ×ØÙÚÛÜİŞßabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            });
            //services.AddDistributedMemoryCache();
            //services.AddSession();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseSession();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600");
                }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Hot}/{id?}");
            });
        }
    }
}
