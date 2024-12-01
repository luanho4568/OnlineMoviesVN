using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository
{
    public class MovieRepository : Repository<Movies>, IMovieRepository
    {
        private readonly ApplicationDbContext _db;
        public MovieRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Movies movie)
        {
            _db.Movies.Update(movie);
        }
    }
}
