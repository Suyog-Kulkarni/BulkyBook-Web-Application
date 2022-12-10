using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
