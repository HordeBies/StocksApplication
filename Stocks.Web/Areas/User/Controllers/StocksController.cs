using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Web.Areas.User.Models;
using Stocks.Web.Configurations;

namespace Stocks.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class StocksController : Controller
    {
        private readonly IFinnhubStocksService finnhubStocksService;
        private readonly TradingOptions tradingOptions;
        public StocksController(IFinnhubStocksService finnhubStocksService, IOptions<TradingOptions> tradingOptions)
        {
            this.finnhubStocksService = finnhubStocksService;
            this.tradingOptions = tradingOptions.Value;
        }

        public async Task<IActionResult> Explore()
        {
            return View();
        }
    }
}
