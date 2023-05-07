using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Core.Services.FinnhubService;
using Stocks.Web.Areas.User.Models;
using Stocks.Web.Configurations;

namespace Stocks.Web.ViewComponents
{
    public class PopularStocksViewComponent : ViewComponent
    {
        private readonly IFinnhubStocksService finnhubStocksService;
        private readonly TradingOptions tradingOptions;
        public PopularStocksViewComponent(IFinnhubStocksService finnhubStocksService, IOptions<TradingOptions> tradingOptions)
        {
            this.finnhubStocksService = finnhubStocksService;
            this.tradingOptions = tradingOptions.Value;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var stocksList = await finnhubStocksService.GetStocks();
            var top25 = tradingOptions.Top25PopularStocks;
            stocksList = stocksList.Where(s => top25.Contains(s["symbol"])).ToList();
            var model = stocksList.Select(s => new Stock() { StockSymbol = s["symbol"], StockName = s["description"] }).OrderBy(s => s.StockName).ToList();

            return View(model);
        }
    }
}
