using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;
using OnlineMoviesVN.Utility.Constant;
using System.Text.RegularExpressions;

namespace OnlineMoviesVN.Areas.Account.Controllers
{
    [Area("Account")]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        public RegisterController(IUnitOfWork unitOfWork, IHttpContextAccessor context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("RegisterSubmit")]
        public async Task<IActionResult> RegisterSubmit()
        {
            try
            {
                string fullName = Request.Form["FullName"];
                string email = Request.Form["Email"];
                string password = Request.Form["Password"];
                string confirmPassword = Request.Form["ConfirmPassword"];
                var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == email && x.AccountType == AccountTypeConstants.Local);
                if (user != null)
                {
                    TempData["Error"] = "Tài khoản này đã có người sử dụng";
                    return RedirectToAction("Index", "Register", new { area = "Account" });
                }
                var passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
                if (!Regex.IsMatch(password, passwordRegex))
                {
                    TempData["Error"] = "Mật khẩu phải chứa ít nhất 8 ký tự, một chữ cái viết hoa, một chữ cái viết thường, một chữ số và một ký tự đặc biệt.";
                    return RedirectToAction("Index", "Register", new { area = "Account" });
                }
                if (confirmPassword != password)
                {
                    TempData["Error"] = "Xác nhận mật khẩu không trùng khớp";
                    return RedirectToAction("Index", "Register", new { area = "Account" });
                }
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                var newUser = new User
                {
                    FullName = fullName,
                    Email = email,
                    Password = hashedPassword,
                    AccountType = AccountTypeConstants.Local,
                    Role = RoleConstants.Member,
                };
                await _unitOfWork.User.AddAsync(newUser);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = "Đăng ký tài khoản thành công!";
                return RedirectToAction("Index", "Login", new { area = "Account" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Index", "Register", new { area = "Account" });
            }
        }
    }
}