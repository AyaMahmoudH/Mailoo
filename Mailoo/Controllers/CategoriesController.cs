using Microsoft.AspNetCore.Mvc;

namespace Mailo.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
