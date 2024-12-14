using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Utility.JwtAuthentication;

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
            return RedirectToAction("Index", "Home");
        }
    }
}
