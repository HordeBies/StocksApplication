using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers
{
    public class HomeController:Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
