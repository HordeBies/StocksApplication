using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Rotativa.AspNetCore;
using Stocks.Core.Domain.Entities;
using Stocks.Core.DTO;
using Stocks.Core.ServiceContracts;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Core.ServiceContracts.StocksService;
using Stocks.Web.Areas.User.Models;
using System.Security.Claims;

namespace Stocks.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly IStocksBuyOrdersService stocksBuyOrdersService;
        private readonly IStocksSellOrdersService stocksSellOrdersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStocksService userStocksService;
        private readonly IFinnhubStockPriceQuoteService finnhubStockQuoteService;
        private readonly IFinnhubCompanyProfileService finnhubCompanyProfileService;
        public PortfolioController(IStocksBuyOrdersService stocksBuyOrdersService, IStocksSellOrdersService stocksSellOrdersService, UserManager<ApplicationUser> userManager, IUserStocksService userStocksService, IFinnhubStockPriceQuoteService finnhubStockQuoteService, IFinnhubCompanyProfileService finnhubCompanyProfileService)
        {
            this.userManager = userManager;
            this.stocksBuyOrdersService = stocksBuyOrdersService;
            this.stocksSellOrdersService = stocksSellOrdersService;
            this.userStocksService = userStocksService;
            this.finnhubStockQuoteService = finnhubStockQuoteService;
            this.finnhubCompanyProfileService = finnhubCompanyProfileService;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            var userStocks = await userStocksService.GetUserStocks(user.Id);
            var userStocksVM = new List<UserStockVM>();
            foreach (var stock in userStocks)
            {
                var symbol = stock.StockId;
                var quote = await finnhubStockQuoteService.GetStockPriceQuote(symbol);
                var profile = await finnhubCompanyProfileService.GetCompanyProfile(symbol);
                var stockVM = new UserStockVM()
                {
                    StockSymbol = symbol,
                    StockName = Convert.ToString(profile["name"].ToString()),
                    SharesOwned = stock.Amount,
                    CurrentPrice = Convert.ToDecimal(quote["c"].ToString()),
                    LastOrder = stock.LastChange,
                };
                stockVM.TotalValue = stockVM.CurrentPrice * stockVM.SharesOwned;
                if (stock.Cost == 0) // TODO: Clear database for empty records
                    stock.Cost = 1;
                stockVM.PercentChange = (stockVM.TotalValue / Convert.ToDecimal(stock.Cost)) - 1m;
                userStocksVM.Add(stockVM);
            }
            PortfolioVM model = new()
            {
                Balance = user.Balance,
                FullName = user.FullName,
                Stocks = userStocksVM,
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> OrderHistory(string? id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<BuyOrderResponse> buyOrders = await stocksBuyOrdersService.GetBuyOrders(userId);
            IEnumerable<SellOrderResponse> sellOrders = await stocksSellOrdersService.GetSellOrders(userId);
            if (!string.IsNullOrEmpty(id))
            {
                buyOrders = buyOrders.Where(r => r.StockSymbol == id);
                sellOrders = sellOrders.Where(r => r.StockSymbol == id);
            }
            var model = new OrderHistoryVM()
            {
                BuyOrders = buyOrders.OrderByDescending(r => r.DateAndTimeOfOrder),
                SellOrders = sellOrders.OrderByDescending(r => r.DateAndTimeOfOrder),
                Symbol = id
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DetailsPDF([FromServices] IWebHostEnvironment hostingEnvironment)
        { // TODO: Make this work

            if (hostingEnvironment.IsProduction())
            {
                TempData["error"] = "This feature is not available in deployment due to Azure restrictions.";
                return RedirectToAction(nameof(Index));
            }
            var user = await userManager.GetUserAsync(User);

            var userStocks = await userStocksService.GetUserStocks(user.Id);
            var userStocksVM = new List<UserStockVM>();
            foreach (var stock in userStocks)
            {
                var symbol = stock.StockId;
                var quote = await finnhubStockQuoteService.GetStockPriceQuote(symbol);
                var profile = await finnhubCompanyProfileService.GetCompanyProfile(symbol);
                var stockVM = new UserStockVM()
                {
                    StockSymbol = symbol,
                    StockName = Convert.ToString(profile["name"].ToString()),
                    SharesOwned = stock.Amount,
                    CurrentPrice = Convert.ToDecimal(quote["c"].ToString()),
                    LastOrder = stock.LastChange,
                };
                stockVM.TotalValue = stockVM.CurrentPrice * stockVM.SharesOwned;
                if (stock.Cost == 0) // TODO: Clear database for empty records
                    stock.Cost = 1;
                stockVM.PercentChange = (stockVM.TotalValue / Convert.ToDecimal(stock.Cost)) - 1m;
                userStocksVM.Add(stockVM);
            }
            PortfolioVM model = new()
            {
                Balance = user.Balance,
                FullName = user.FullName,
                Stocks = userStocksVM,
            };

            return new ViewAsPdf("DetailsPDF", model, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
        [HttpGet]
        public async Task<IActionResult> OrdersPDF(string? id, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsProduction())
            {
                TempData["error"] = "This feature is not available in deployment due to Azure restrictions.";
                return RedirectToAction(nameof(Index));
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await stocksBuyOrdersService.GetBuyOrders(userId));
            orders.AddRange(await stocksSellOrdersService.GetSellOrders(userId));
            if (!string.IsNullOrEmpty(id))
            {
                orders = orders.Where(r => r.StockSymbol == id).ToList();
            }

            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
        [HttpGet]
        public async Task<IActionResult> MailOrdersPDF([FromServices] IEmailAttachmentSender emailSender, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsProduction())
            {
                TempData["error"] = "This feature is not available in deployment due to Azure restrictions.";
                return RedirectToAction(nameof(Index));
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await stocksBuyOrdersService.GetBuyOrders(userId));
            orders.AddRange(await stocksSellOrdersService.GetSellOrders(userId));

            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            var view = new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
            byte[] applicationPDFData = await view.BuildFile(ControllerContext);
            Stream stream = new MemoryStream(applicationPDFData);
            await emailSender.SendEmailAsync(userManager.GetUserName(User)!, "BiesStock Portfolio", "Here is your buy&sell order history.", stream,"Order History.pdf");
            TempData["success"] = "Email sent successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
