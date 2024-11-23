using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository
{
    public class UserActivityLogRepository : Repository<UserActivityLog>, IUserActivityLogRepository
    {
        private readonly ApplicationDbContext _db;
        public UserActivityLogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
