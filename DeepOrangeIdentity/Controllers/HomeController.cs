using Microsoft.AspNetCore.Mvc;

namespace DeepOrangeIdentity.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
