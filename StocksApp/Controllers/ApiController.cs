using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        private readonly IConfiguration configuration;

        public ApiController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        [Route("finnhub/token")]
        public IActionResult GetFinnhubToken() //TODO: use authentication to prevent 3rd party to acces this endpoint
        {
            if (configuration["FinnhubToken"] == null)
            {
                return NotFound();
            }
            return Content(configuration["FinnhubToken"]!);
        }
    }
}
