using Microsoft.AspNetCore.Mvc;

namespace AppPartsDemo.Controllers
{
    [Area("Roles")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ContentResult GetOk()
        {
            return Content("Ok");
        }
    }
}
