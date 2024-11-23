using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository
{
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        private readonly ApplicationDbContext _db;
        public DistrictRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
