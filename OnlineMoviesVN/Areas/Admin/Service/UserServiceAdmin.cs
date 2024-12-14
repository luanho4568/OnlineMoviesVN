using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.Areas.Admin.Service
{
    public class UserServiceAdmin
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public UserServiceAdmin(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<User>> GetUserService(bool active)
        {
            var listUser = await _unitOfWork.User.GetAllAsync(x => x.IsActive == active);
            return listUser.ToList();
        }

    }
}
