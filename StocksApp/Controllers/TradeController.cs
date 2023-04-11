using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using ServiceContracts;
using Services;
using ServiceContracts.DTO;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions tradingOptions;
        private readonly IFinnhubService finnhubService;
        private readonly IStocksService stocksService;

        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IStocksService stocksService)
        {
            this.tradingOptions = tradingOptions.Value;
            this.finnhubService = finnhubService;
            this.stocksService = stocksService;
        }
        [HttpGet]
        [Route("/")]
        [Route("[action]")]
        [Route("~/[controller]")]
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(tradingOptions.DefaultStockSymbol))
            {
                tradingOptions.DefaultStockSymbol = "MSFT";
            }
            var quote = await finnhubService.GetStockPriceQuote(tradingOptions.DefaultStockSymbol);
            var profile = await finnhubService.GetCompanyProfile(tradingOptions.DefaultStockSymbol);
            var model = new StockTrade()
            {
                Price = Convert.ToDouble(quote["c"].ToString()),
                StockName = profile["name"].ToString(),
                StockSymbol = tradingOptions.DefaultStockSymbol
            };
            return View(model);
        }

        [Route("[action]")]
        public IActionResult BuyOrder(BuyOrderRequest request)
        {
            request.DateAndTimeOfOrder = DateTime.Now;
            ModelState.Clear();
            TryValidateModel(request);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var stockTrade = new StockTrade() { StockName = request.StockName, Quantity = request.Quantity, StockSymbol = request.StockSymbol };
                return View("Index", stockTrade);
            }

            var response = stocksService.CreateBuyOrder(request);

            return RedirectToAction(nameof(Orders));
        }
        [Route("[action]")]
        public IActionResult SellOrder(SellOrderRequest request)
        {
            request.DateAndTimeOfOrder = DateTime.Now;
            ModelState.Clear();
            TryValidateModel(request);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var stockTrade = new StockTrade() { StockName = request.StockName, Quantity = request.Quantity, StockSymbol = request.StockSymbol };
                return View("Index", stockTrade);
            }

            var response = stocksService.CreateSellOrder(request);

            return RedirectToAction(nameof(Orders));
        }
        [Route("[action]")]
        public IActionResult Orders()
        {
            var buyOrders = stocksService.GetBuyOrders();
            var sellOrders = stocksService.GetSellOrders();

            var orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrders };

            return View(orders);
        }
    }
}
