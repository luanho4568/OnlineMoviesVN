using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository
{
    public class WardRepository : Repository<Ward>, IWardRepository
    {
        private readonly ApplicationDbContext _db;
        public WardRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
