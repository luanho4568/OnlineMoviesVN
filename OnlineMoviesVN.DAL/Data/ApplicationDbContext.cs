using Microsoft.EntityFrameworkCore;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Các DbSet cho các bảng trong cơ sở dữ liệu
        public DbSet<User> Users { get; set; }
        public DbSet<ContactUsRequest> ContactUsRequest { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Episodes> Episodes { get; set; }
        public DbSet<UsersPermission> UsersPermissions { get; set; }
        public DbSet<UsersPermissionsGranted> UsersPermissionsGranteds { get; set; }
    }
}
