using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.JwtAuthentication;
using OnlineMoviesVN.Utility.Sessions;

namespace OnlineMoviesVN.Areas.Account.Controllers
{
    [Area("Account")]
    [AllowAnonymous]

    public class GoogleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public GoogleController(IUnitOfWork unitOfWork, JwtService jwtService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _configuration = configuration;
        }
        [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme)]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleCallback", "Google", new { area = "Account" });
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleCallback()
        {
            var expireTime = _configuration.GetValue<int>("Jwt:TokenExpiryInDay");
            var authenticateResult = await HttpContext.AuthenticateAsync();
            if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
            {
                TempData["Error"] = "Đăng nhập Google thất bại!";
                return RedirectToAction("Index", "Login", new { area = "Account" });
            }
            var email = authenticateResult.Principal.FindFirst(x => x.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var fullName = authenticateResult.Principal.FindFirst(x => x.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Không thể lấy thông tin email từ tài khoản Google.";
                return RedirectToAction("Index", "Login", new { area = "Account" });
            }
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == email && x.AccountType == AccountTypeConstants.Google);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    FullName = fullName,
                    IsStatus = UserStatusConstants.Status,
                    Role = RoleConstants.Member,
                    AccountType = AccountTypeConstants.Google,
                    LastLogin = DateTime.Now
                };
                await _unitOfWork.User.AddAsync(user);
            }
            else
            {
                user.LastLogin = DateTime.Now;
            }
            await _unitOfWork.SaveAsync();
            HttpContext.Session.Set(StorageConstants.KeySessionUser, user);
            return RedirectToAction("Index", "Home", new { area = "Client" });
        }
    }
}
