using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.Database.Models;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.Cookies;
using OnlineMoviesVN.Utility.Sessions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineMoviesVN.Utility.JwtAuthentication
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _db;

        public JwtService(IConfiguration configuration, ApplicationDbContext db)

        {

            _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
            _db = db;

        }
        // Tạo mới token
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>

        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email,user.Email == null ? string.Empty : user.Email),
            new Claim(ClaimTypes.Role, user.Role),
        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor

            {

                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.Now.AddDays(_jwtSettings.TokenExpiryInDay),

                SigningCredentials = credentials,

                Issuer = _jwtSettings.Issuer,

                Audience = _jwtSettings.Audience

            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        // Giải mã token
        public IDictionary<string, string> DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsDictionary = new Dictionary<string, string>();

            try
            {
                // Tham số xác thực token
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, // Kiểm tra thời gian hết hạn
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    ClockSkew = TimeSpan.Zero // Không cho phép thời gian trễ
                };

                // Xác thực token
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Nếu token hợp lệ, giải mã các claims
                var jwtToken = tokenHandler.ReadJwtToken(token);
                foreach (var claim in jwtToken.Claims)
                {
                    claimsDictionary[claim.Type] = claim.Value;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                throw new SecurityTokenException("Token đã hết hạn");
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Token không hợp lệ: " + ex.Message);
            }

            return claimsDictionary;
        }


        //Kiểm tra hết hạn token
        public bool ValidateTokenExpiration(ClaimsPrincipal principal, HttpContext context)
        {
            var claimsIdentity = principal.Identity as ClaimsIdentity;

            var expClaim = claimsIdentity?.FindFirst("exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out long exp))
            {
                // Chuyển đổi exp thành thời gian UTC
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                // Kiểm tra nếu token đã hết hạn
                if (DateTime.UtcNow > expirationTime)
                {
                    context.Response.DeleteCookie(StorageConstants.KeyTokenCookie);
                    var sessionUser = context.Session.Get<User>(StorageConstants.KeySessionUser);
                    if (sessionUser != null)
                    {
                        context.Session.RemoveSession(StorageConstants.KeySessionUser);
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        //Xử lý gia hạn thêm session
        public async Task RenewSession(HttpContext context)
        {
            var sessionUser = context.Session.Get<User>(StorageConstants.KeySessionUser);
            var token = context.Request.GetCookie(StorageConstants.KeyTokenCookie);
            if (!string.IsNullOrEmpty(token))
            {
                if (sessionUser == null)
                {
                    try
                    {
                        var claims = DecodeToken(token);
                        if (!claims.TryGetValue("nameid", out string nameid))
                        {
                            Console.WriteLine("Không có token");
                            return;
                        }
                        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id.ToString() == nameid);
                        if (user == null)
                        {
                            Console.WriteLine("Không tìm thấy user");
                            return;
                        }
                        context.Session.Set(StorageConstants.KeySessionUser, user);
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine("Lỗi khi giải mã token hoặc lấy thông tin người dùng: " + e.Message);
                    }
                }
            }
        }

    }
}
