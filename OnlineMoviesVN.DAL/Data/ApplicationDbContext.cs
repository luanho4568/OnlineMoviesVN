using Microsoft.EntityFrameworkCore;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<AdministrativeRegion> AdministrativeRegions { get; set; }
        public DbSet<AdministrativeUnit> AdministrativeUnits { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserActivityLog> UserActivityLogs { get; set; }
    }
}
