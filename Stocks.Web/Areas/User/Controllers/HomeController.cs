using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Stocks.Web.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
                return View("UserHome");

            return View("GuestHome");
        }
        public IActionResult Beginner()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("/Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
                ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StatusCode = HttpContext.Response.StatusCode;
            return View();
        }
    }
}
