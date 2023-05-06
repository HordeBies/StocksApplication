using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Web.Areas.User.Models;
using Stocks.Web.Configurations;

namespace Stocks.Web.Areas.User.Controllers
{
    [Area("User")]
    public class StocksController : Controller
    {
        private readonly IFinnhubStocksService finnhubStocksService;
        private readonly TradingOptions tradingOptions;
        public StocksController(IFinnhubStocksService finnhubStocksService, IOptions<TradingOptions> tradingOptions)
        {
            this.finnhubStocksService = finnhubStocksService;
            this.tradingOptions = tradingOptions.Value;
        }

        public async Task<IActionResult> Explore(string? id, bool showAll = false)
        {
            var stocksList = await finnhubStocksService.GetStocks();
            var top25 = tradingOptions.Top25PopularStocks;
            if (!showAll)
                stocksList = stocksList.Where(s => top25.Contains(s["symbol"])).ToList();
            var model = stocksList.Select(s => new Stock() { StockSymbol = s["symbol"], StockName = s["description"] }).OrderBy(s => s.StockName).ToList();

            ViewBag.Stock = id;
            ViewBag.ShowAll = showAll;
            return View(model);
        }
    }
}
