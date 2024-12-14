using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMoviesVN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN, MANAGER")]
    public class MoviesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
