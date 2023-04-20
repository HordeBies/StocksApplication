using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using ServiceContracts;
using Services;
using ServiceContracts.DTO;
using Rotativa.AspNetCore;

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
        [Route("[action]/{stockSymbol}")]
        [Route("~/[controller]/{stockSymbol}")]
        public async Task<IActionResult> Index(string? stockSymbol)
        {
            if(string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";

            var quote = await finnhubService.GetStockPriceQuote(stockSymbol);
            var profile = await finnhubService.GetCompanyProfile(stockSymbol);
            var model = new StockTrade()
            {
                Price = Convert.ToDouble(quote["c"].ToString()),
                Quantity = tradingOptions.DefaultOrderQuantity ?? 0,
                StockName = profile["name"].ToString(),
                StockSymbol = stockSymbol
            };
            return View(model);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest request)
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

            var response = await stocksService.CreateBuyOrder(request);

            return RedirectToAction(nameof(Orders));
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SellOrder(SellOrderRequest request)
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

            var response = await stocksService.CreateSellOrder(request);

            return RedirectToAction(nameof(Orders));
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            var buyOrders = await stocksService.GetBuyOrders();
            var sellOrders = await stocksService.GetSellOrders();

            var orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrders };

            return View(orders);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await stocksService.GetBuyOrders());
            orders.AddRange(await stocksService.GetSellOrders());
            
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
