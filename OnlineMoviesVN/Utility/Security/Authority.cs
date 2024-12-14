using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.Database.Models;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.Sessions;
using static OnlineMoviesVN.Utility.Enums.Enums;

namespace OnlineMoviesVN.Utility.Security
{
    public class Authority
    {
        public static async Task<Dictionary<string, bool>> GetAccessAuthority(IHttpContextAccessor httpContext, User curMem = null, bool reload = false)
        {
            var context = httpContext.HttpContext;
            var access = new Dictionary<string, bool>();
            try
            {
                if (curMem == null)
                {
                    curMem = context.Session.Get<User>(StorageConstants.KeySessionUser);
                    if (curMem == null)
                    {
                        return access;
                    }
                }
                string memberRole = curMem.Role;
                ApplicationDbContext db = new();
                if (memberRole == UserRole.Admin.ToString())
                {
                    foreach (var item in db.UsersPermissions.ToList())
                    {
                        access.Add(item.Code, true);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(memberRole))
                {
                    foreach (var item in db.UsersPermissionsGranteds.Where(p => p.UserRole.Equals(memberRole.Trim()) && p.Active == true).OrderBy(p => p.PermissionCode).ToList())
                    {
                        access.Add(item.PermissionCode, true);
                    }
                }
                return access;

            }
            catch (Exception)
            {
                return access;
            }
        }
        public static async Task<bool> CheckAccess(IHttpContextAccessor httpContext, string code)
        {
            var accessAuthority = await GetAccessAuthority(httpContext);
            if (accessAuthority == null)
            {
                return false;
            }

            return accessAuthority.TryGetValue(code, out var hasAccess) && hasAccess;
        }
    }
}
