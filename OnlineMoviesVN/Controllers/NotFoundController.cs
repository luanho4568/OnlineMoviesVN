using Microsoft.AspNetCore.Mvc;

namespace OnlineMoviesVN.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
