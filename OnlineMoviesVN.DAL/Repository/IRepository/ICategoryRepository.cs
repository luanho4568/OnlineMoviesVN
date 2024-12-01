using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
