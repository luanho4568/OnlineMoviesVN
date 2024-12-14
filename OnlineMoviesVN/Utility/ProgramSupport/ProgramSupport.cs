
using Microsoft.EntityFrameworkCore;
using OnlineMoviesVN.Areas.Account.Service;
using OnlineMoviesVN.Areas.Admin.Service;
using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Utility.JwtAuthentication;

namespace OnlineMoviesVN.Utility.ProgramSupport
{
    public static class ProgramSupport
    {
        public static void ProgramRouteSp(WebApplication app)
        {
            app.MapControllerRoute(
                name: "MyArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { area = "Client" }
            );
            app.MapControllerRoute(
              name: "NotFound",
              pattern: "{controller=Home}/{action=Index}/{id?}");
        }
        public static void ProgramBuildAddScoped(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<GoogleService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<UserServiceAdmin>();

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));
        }
    }
}
