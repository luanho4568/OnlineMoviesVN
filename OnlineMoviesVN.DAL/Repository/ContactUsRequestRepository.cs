using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository
{
    public class ContactUsRequestRepository : Repository<ContactUsRequest>, IContactUsRequestRepository
    {
        private readonly ApplicationDbContext _db;
        public ContactUsRequestRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
