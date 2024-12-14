using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMoviesVN.Areas.Admin.Service;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Utility.Security;

namespace OnlineMoviesVN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN, MANAGER")]
    public class UsersController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserServiceAdmin _userServiceAdmin;
        public UsersController(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork, UserServiceAdmin userServiceAdmin)
        {
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
            _userServiceAdmin = userServiceAdmin;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> GetUserActive()
        {
            var access = await Authority.GetAccessAuthority(_contextAccessor);
            //if (!access.ContainsKey("update_customer"))
            //{
            //    TempData["Error"] = "Bạn không có quyền truy cập chức năng này.";
            //    return RedirectToAction("index", "home", new { area = "Admin" });
            //}
            var data = await _userServiceAdmin.GetUserService(true);
            if (data == null)
            {
                TempData["Error"] = "Tải dữ liệu thất bại!";
            }
            return PartialView("_UserDataTable", data);
        }
    }
}
