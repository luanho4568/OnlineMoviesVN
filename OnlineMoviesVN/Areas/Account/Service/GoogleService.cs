using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Database.Models;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.Cookies;
using OnlineMoviesVN.Utility.JwtAuthentication;
using OnlineMoviesVN.Utility.Sessions;
using System.Security.Claims;

namespace OnlineMoviesVN.Areas.Account.Service
{
    public class GoogleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly JwtService jwtService;
        public GoogleService(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
        }
        public async Task LoginGoogleService(ClaimsPrincipal? principal, HttpContext context)
        {
            // Lấy thông tin email từ Google
            var email = principal?.FindFirstValue(ClaimTypes.Email);
            var fullName = principal?.FindFirstValue(ClaimTypes.Name);
            // Xác thực trong cơ sở dữ liệu
            var user = await unitOfWork.User.GetFirstOrDefaultAsync(u => u.Email == email && u.AccountType == AccountTypeConstants.Google);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    FullName = fullName,
                    IsStatus = UserStatusConstants.Status,
                    Role = RoleConstants.Customer,
                    AccountType = AccountTypeConstants.Google,
                    LastLogin = DateTime.Now
                };
                await unitOfWork.User.AddAsync(user);
            }
            else
            {
                user.LastLogin = DateTime.Now;
            }
            await unitOfWork.SaveAsync();

            var identity = principal?.Identity as ClaimsIdentity;
            identity?.AddClaim(new Claim(ClaimTypes.Role, user.Role));

            // Lưu vào session hoặc cookie
            var token = jwtService.GenerateToken(user);
            context.Response.SetCookie(StorageConstants.KeyTokenCookie, token, 7);
            context.Session.Set(StorageConstants.KeySessionUser, user);
        }
    }
}
