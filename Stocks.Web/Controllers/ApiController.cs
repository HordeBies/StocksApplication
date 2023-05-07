using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Web.Areas.User.Models;

namespace Stocks.Web.Controllers
{
    [Route("/[controller]")]
    public class ApiController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IFinnhubStocksService stocksService;

        public ApiController(IConfiguration configuration, IFinnhubStocksService stocksService)
        {
            this.configuration = configuration;
            this.stocksService = stocksService;
        }
        [HttpGet]
        [Route("finnhub/token")]
        [Authorize]
        public IActionResult GetFinnhubToken() //TODO: use authentication to prevent 3rd party to acces this endpoint
        {
            if (configuration["FinnhubToken"] == null)
            {
                return NotFound();
            }
            return Content(configuration["FinnhubToken"]!);
        }
        [HttpGet]
        [Route("finnhub/stocknames/all")]
        public async Task<JsonResult> GetStocksNamesList()
        {
            var stockDict = await stocksService.GetStocks();
            var stockList = stockDict?.Select(s => s["symbol"]).OrderBy(s => s.Length);
            return Json(stockList);
        }

        [HttpGet]
        [Route("finnhub/stock/all")]
        public async Task<JsonResult> GetAllStocks()
        {
            var stocksList = await stocksService.GetStocks();
            var model = stocksList.Select(s => new Stock() { StockSymbol = s["symbol"], StockName = s["description"] }).ToList();
            return Json(new {data = model });
        }

        [HttpGet]
        [Route("finnhub/stock/")]
        public async Task<IActionResult> StockDetailVC(string? stockSymbol)
        {
            return ViewComponent("SelectedStock", new { stockSymbol });
        }

        [HttpGet]
        [Route("finnhub/stock/popular")]
        public async Task<IActionResult> Top25PopularStock()
        {
            return ViewComponent("PopularStocks");
        }
    }
}
