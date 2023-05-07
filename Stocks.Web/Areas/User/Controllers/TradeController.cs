using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Stocks.Core.Domain.Entities;
using Stocks.Core.DTO;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Core.ServiceContracts.StocksService;
using Stocks.Web.Areas.User.Models;
using Stocks.Web.Configurations;
using Stocks.Web.Filters.ActionFilters;
using System.Security.Claims;

namespace Stocks.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class TradeController : Controller
    {
        private readonly TradingOptions tradingOptions;
        private readonly IStocksBuyOrdersService stocksBuyOrdersService;
        private readonly IStocksSellOrdersService stocksSellOrdersService;
        private readonly UserManager<ApplicationUser> userManager;

        public TradeController(UserManager<ApplicationUser> userManager,IOptions<TradingOptions> tradingOptions, IStocksBuyOrdersService stocksBuyOrdersService, IStocksSellOrdersService stocksSellOrdersService) //Injected services via constructor because they are used in multiple methods
        {
            this.userManager = userManager;
            this.tradingOptions = tradingOptions.Value;
            this.stocksBuyOrdersService = stocksBuyOrdersService;
            this.stocksSellOrdersService = stocksSellOrdersService;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IFinnhubCompanyProfileService finnhubCompanyProfileService, [FromServices] IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService, [FromServices] IUserStocksService userStocksService, string? id) //Injected services via parameter because they are not used in other methods
        {
            
            if (string.IsNullOrEmpty(id))
                return View(new StockTrade());

            var quote = await finnhubStockPriceQuoteService.GetStockPriceQuote(id);
            var profile = await finnhubCompanyProfileService.GetCompanyProfile(id);
            var model = new StockTrade()
            {
                Price = Convert.ToDouble(quote["c"].ToString()),
                Quantity = tradingOptions.DefaultOrderQuantity ?? 0,
                StockName = profile["name"].ToString(),
                StockSymbol = id,
                PreviousClosedPrice = Convert.ToDouble(quote["pc"].ToString()),
                ChangePercent = Convert.ToDouble(quote["dp"].ToString()),
                DailyChange = Convert.ToDouble(quote["d"].ToString())
            };

            var user = await userManager.GetUserAsync(User);
            var userStock = await userStocksService.GetUserStock(user.Id, id);
            model.SharesOwned = userStock?.Amount ?? 0;

            ViewBag.Balance = user.Balance;
            return View(model);
        }

        [HttpPost]
        [CreateOrderActionFilter]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await stocksBuyOrdersService.CreateBuyOrder(orderRequest,userId);

            TempData["success"] = "Shares purchased successfully";
            return RedirectToAction(nameof(Index), new { id = orderRequest.StockSymbol });
        }
        [HttpPost]
        [CreateOrderActionFilter]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await stocksSellOrdersService.CreateSellOrder(orderRequest, userId);

            TempData["success"] = "Shares sold successfully";
            return RedirectToAction(nameof(Index), new { id = orderRequest.StockSymbol });
        }
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var buyOrders = await stocksBuyOrdersService.GetBuyOrders(userId);
            var sellOrders = await stocksSellOrdersService.GetSellOrders(userId);

            var orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrders };

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> OrdersPDF()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await stocksBuyOrdersService.GetBuyOrders(userId));
            orders.AddRange(await stocksSellOrdersService.GetSellOrders(userId));

            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
