using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _db;
        public CountryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Country country)
        {
            _db.Countries.Update(country);
        }
    }
}
