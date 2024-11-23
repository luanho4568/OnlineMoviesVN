using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.Cookies;
using OnlineMoviesVN.Utility.JwtAuthentication;
using OnlineMoviesVN.Utility.Sessions;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OnlineMoviesVN.Areas.Account.Controllers
{
    [Area("Account")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        public LoginController(JwtService jwtService, IConfiguration configuration, IUnitOfWork unitOfWork, IHttpContextAccessor context)
        {
            _jwtService = jwtService;
            _configuration = configuration;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var user = HttpContext.Session.Get<User>(StorageConstants.KeySessionUser);
            var remember = Request.GetCookie(StorageConstants.KeyRememberCookie);
            if (user == null)
            {
                if (remember != null)
                {
                    var loginInfo = JsonSerializer.Deserialize<JsonObject>(remember);
                    var email = loginInfo?["email"]?.ToString();
                    var password = loginInfo?["password"]?.ToString();
                    ViewData["Email"] = email;
                    ViewData["Password"] = password;
                    ViewData["Remember"] = "Checked";
                }
                else
                {
                    ViewData["Email"] = string.Empty;
                    ViewData["Password"] = string.Empty;
                    ViewData["Remember"] = string.Empty;
                }
            }
            if (user != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return View();
        }

        [HttpPost]
        [ActionName("LoginSubmit")]
        public async Task<IActionResult> LoginSubmit()
        {
            try
            {
                var expireTime = _configuration.GetValue<int>("Jwt:TokenExpiryInDay");
                string email = Request.Form["Email"];
                string password = Request.Form["Password"];
                bool remember = Request.Form.ContainsKey("RememberMe");
                var login = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Email == email);
                if (login == null)
                {
                    TempData["Error"] = "Tài khoản hoặc mật khẩu không chính xác";
                    return RedirectToAction("Index", "Login", new { area = "Account" });
                }
                var isPassword = BCrypt.Net.BCrypt.Verify(password, login.Password);
                if (isPassword)
                {
                    login.LastLogin = DateTime.Now;
                    await _unitOfWork.SaveAsync();
                    var token = _jwtService.GenerateToken(login);
                    Response.SetCookie(StorageConstants.KeyTokenCookie, token, expireTime);
                    HttpContext.Session.Set(StorageConstants.KeySessionUser, login);
                    var loginInfo = new { email, password };
                    var loginInfoJson = JsonSerializer.Serialize(loginInfo);
                    if (remember)
                    {
                        Response.SetCookie(StorageConstants.KeyRememberCookie, loginInfoJson, expireTime);
                    }
                    else
                    {
                        var rememberCookie = Request.GetCookie(StorageConstants.KeyRememberCookie);
                        if (rememberCookie != null)
                        {
                            Response.DeleteCookie(StorageConstants.KeyRememberCookie);
                        }
                    }
                    TempData["Success"] = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    TempData["Error"] = "Tài khoản hoặc mật khẩu không chính xác";
                    return RedirectToAction("Index", "Login", new { area = "Account" });
                }

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                return View();
            }
        }
        public async Task<IActionResult> Logout()
        {
            Response.DeleteCookie(StorageConstants.KeyTokenCookie);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login", new { area = "Account" });
        }
    }
}
