using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.Cookies;
using OnlineMoviesVN.Utility.JwtAuthentication;
using OnlineMoviesVN.Utility.Sessions;
using System.Security.Claims;

namespace OnlineMoviesVN.Areas.Account.Controllers
{
    [Area("Account")]
    [AllowAnonymous]
    public class FacebookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public FacebookController(IUnitOfWork unitOfWork, JwtService jwtService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        [Authorize(AuthenticationSchemes = FacebookDefaults.AuthenticationScheme)]
        public IActionResult LoginWithFacebook()
        {
            var redirectUrl = Url.Action("FacebookCallback", "Facebook", new { area = "Account" });
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookCallback()
        {
            var expireTime = _configuration.GetValue<int>("Jwt:TokenExpiryInDay");
            var info = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (info?.Principal == null)
            {
                TempData["Error"] = "Đăng nhập Facebook thất bại!";
                return RedirectToAction("Index", "Login", new { area = "Account" });
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var fullName = info.Principal.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Không thể lấy thông tin email từ tài khoản Facebook.";
                return RedirectToAction("Index", "Login", new { area = "Account" });
            }

            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == email && x.AccountType == AccountTypeConstants.Facebook);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    FullName = fullName,
                    Role = RoleConstants.Customer,
                    IsStatus = UserStatusConstants.Status,
                    AccountType = AccountTypeConstants.Facebook,
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
            var token = _jwtService.GenerateToken(user);
            HttpContext.Response.SetCookie(StorageConstants.KeyTokenCookie, token, 7);
            return RedirectToAction("Index", "Home");
        }
    }
}
