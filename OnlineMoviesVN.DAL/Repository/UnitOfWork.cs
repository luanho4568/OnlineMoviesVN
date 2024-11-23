using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;

namespace OnlineMoviesVN.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        public IProvinceRepository Province { get; private set; }

        public IDistrictRepository District { get; private set; }

        public IWardRepository Ward { get; private set; }

        public IUserRepository User { get; private set; }


        public IContactUsRequestRepository ContactUsRequest { get; private set; }

        public IUserActivityLogRepository UserActivityLog { get; private set; }

        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Province = new ProvinceRepository(_db);
            District = new DistrictRepository(_db);
            Ward = new WardRepository(_db);
            User = new UserRepository(_db);
            ContactUsRequest = new ContactUsRequestRepository(_db);
            UserActivityLog = new UserActivityLogRepository(_db);
        }


        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
