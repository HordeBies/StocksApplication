using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using ServiceContracts;
using Services;
using ServiceContracts.DTO;
using Rotativa.AspNetCore;
using StocksApp.Filters.ActionFilters;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions tradingOptions;
        private readonly IStocksBuyOrdersService stocksBuyOrdersService;
        private readonly IStocksSellOrdersService stocksSellOrdersService;

        public TradeController(IOptions<TradingOptions> tradingOptions, IStocksBuyOrdersService stocksBuyOrdersService, IStocksSellOrdersService stocksSellOrdersService) //Injected services via constructor because they are used in multiple methods
        {
            this.tradingOptions = tradingOptions.Value;
            this.stocksBuyOrdersService = stocksBuyOrdersService;
            this.stocksSellOrdersService = stocksSellOrdersService;
        }
        [HttpGet]
        [Route("[action]/{stockSymbol}")]
        [Route("~/[controller]/{stockSymbol}")]
        public async Task<IActionResult> Index([FromServices] IFinnhubCompanyProfileService finnhubCompanyProfileService,[FromServices] IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService, string? stockSymbol) //Injected services via parameter because they are not used in other methods
        {
            if(string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";

            var quote = await finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);
            var profile = await finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
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
        [CreateOrderActionFilter]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            var response = await stocksBuyOrdersService.CreateBuyOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }
        [HttpPost]
        [Route("[action]")]
        [CreateOrderActionFilter]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            var response = await stocksSellOrdersService.CreateSellOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            var buyOrders = await stocksBuyOrdersService.GetBuyOrders();
            var sellOrders = await stocksSellOrdersService.GetSellOrders();

            var orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrders };

            return View(orders);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await stocksBuyOrdersService.GetBuyOrders());
            orders.AddRange(await stocksSellOrdersService.GetSellOrders());
            
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
