using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMoviesVN.Database.Models;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.Sessions;

namespace OnlineMoviesVN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var user = HttpContext.Session.Get<User>(StorageConstants.KeySessionUser);
            Console.WriteLine(user);
            return View();
        }
    }
}
