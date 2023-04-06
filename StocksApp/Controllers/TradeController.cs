using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.ServiceContracts;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly TradingOptions tradingOptions;
        private readonly IFinnhubService finnhubService;

        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService)
        {
            this.tradingOptions = tradingOptions.Value;
            this.finnhubService = finnhubService;
        }
        [HttpGet]
        [Route("/trade")]
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(tradingOptions.DefaultStockSymbol))
            {
                tradingOptions.DefaultStockSymbol = "MSFT";
            }
            var quote = await finnhubService.GetQuote(tradingOptions.DefaultStockSymbol);
            var profile = await finnhubService.GetCompanyProfile(tradingOptions.DefaultStockSymbol);
            var model = new StockTrade()
            {
                Price = Convert.ToDouble(quote["c"].ToString()),
                StockName = profile["name"].ToString(),
                StockSymbol = tradingOptions.DefaultStockSymbol
            };
            return View(model);
        }
    }
}
