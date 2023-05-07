using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Stocks.Core.Domain.Entities;
using Stocks.Core.DTO;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Core.ServiceContracts.StocksService;
using Stocks.Core.Services.StocksService;
using Stocks.Web.Areas.User.Models;
using System.Security.Claims;

namespace Stocks.Web.Areas.User.Controllers
{
    [Area("User")]
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
                stockVM.PercentChange = (stockVM.TotalValue / Convert.ToDecimal(stock.Cost)) - 1m ;
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
    }
}
