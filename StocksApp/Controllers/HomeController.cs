using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
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
