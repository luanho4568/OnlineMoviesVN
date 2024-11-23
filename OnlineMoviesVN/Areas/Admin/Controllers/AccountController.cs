using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.Cookies;
using OnlineMoviesVN.Utility.JwtAuthentication;
using OnlineMoviesVN.Utility.Sessions;

namespace OnlineMoviesVN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        public AccountController(JwtService jwtService, IConfiguration configuration, IUnitOfWork unitOfWork, IHttpContextAccessor context)
        {
            _jwtService = jwtService;
            _configuration = configuration;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var user = HttpContext.Session.Get<User>(StorageConstants.KeySessionUser);
            if (user != null)
            {
                return RedirectToAction(nameof(Index), "Home", new { area = "Admin" });
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Login")]
        public async Task<IActionResult> LoginSubmit()
        {
            try
            {
                var expireTime = _configuration.GetValue<int>("Jwt:TokenExpiryInDay");
                string email = Request.Form["Email"];
                string password = Request.Form["Password"];

                var login = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Email == email);
                if (login == null)
                {
                    TempData["Error"] = "Tài khoản hoặc mật khẩu không chính xác";
                    return View();
                }
                var isPassword = BCrypt.Net.BCrypt.Verify(password, login.Password);
                if (isPassword)
                {

                    await _unitOfWork.UserActivityLog.AddAsync(new UserActivityLog
                    {
                        UserId = login.Id,
                        Email = login.Email,
                        Action = RoleConstants.Admin,
                        ActivityType = ActiveTypeConstants.Login,
                        IPAddress = _context?.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                    });
                    await _unitOfWork.SaveAsync();
                    var token = _jwtService.GenerateToken(login);
                    Response.SetCookie(StorageConstants.KeyTokenCookie, token, expireTime);
                    HttpContext.Session.Set(StorageConstants.KeySessionUser, login);
                    TempData["Success"] = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    TempData["Error"] = "Tài khoản hoặc mật khẩu không chính xác";
                    return View();
                }

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                return View();
            }
        }
    }
}
