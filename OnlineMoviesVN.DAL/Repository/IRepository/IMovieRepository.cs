using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository.IRepository
{
    public interface IMovieRepository : IRepository<Movies>
    {
        void Update(Movies movie);
    }
}
