using Microsoft.EntityFrameworkCore;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<ContactUsRequest> ContactUsRequest { get; set; }
    }
}
