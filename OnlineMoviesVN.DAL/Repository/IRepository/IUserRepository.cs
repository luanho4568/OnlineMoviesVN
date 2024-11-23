using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User user);
    }
}
