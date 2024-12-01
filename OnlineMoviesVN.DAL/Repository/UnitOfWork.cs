using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;

namespace OnlineMoviesVN.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {


        public IUserRepository User { get; private set; }


        public IContactUsRequestRepository ContactUsRequest { get; private set; }

        public IMovieRepository Movie { get; private set; }

        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            User = new UserRepository(_db);
            ContactUsRequest = new ContactUsRequestRepository(_db);
            Movie = new MovieRepository(_db);
        }


        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
