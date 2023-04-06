using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.ServiceContracts;

namespace StocksApp.Controllers
{
    public class HomeController:Controller
    {
        private readonly IFinnhubService finnhubService;
        private readonly TradingOptions tradingOptions;

        public HomeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            this.finnhubService = finnhubService;
            this.tradingOptions = tradingOptions.Value;
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(tradingOptions.DefaultStockSymbol))
            {
                tradingOptions.DefaultStockSymbol = "MSFT";
            }
            var quote = await finnhubService.GetQuote(tradingOptions.DefaultStockSymbol);
            Stock stock = new() { 
                StockSymbol = tradingOptions.DefaultStockSymbol, 
                CurrentPrice = Convert.ToDouble(quote["c"].ToString()), 
                HighestPrice = Convert.ToDouble(quote["h"].ToString()), 
                LowestPrice= Convert.ToDouble(quote["l"].ToString()), 
                OpenPrice = Convert.ToDouble(quote["o"].ToString()) 
            };
            return View(stock);
        }
    }
}
