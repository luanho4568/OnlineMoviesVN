using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
