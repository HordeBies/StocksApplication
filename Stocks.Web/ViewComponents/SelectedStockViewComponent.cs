using Microsoft.AspNetCore.Mvc;
using Stocks.Core.ServiceContracts.FinnhubService;

namespace Stocks.Web.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubCompanyProfileService finnhubCompanyProfileService;
        private readonly IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService;
        public SelectedStockViewComponent(IFinnhubCompanyProfileService finnhubCompanyProfileService, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService)
        {
            this.finnhubCompanyProfileService = finnhubCompanyProfileService;
            this.finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
                return Content("");
            var profile = await finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
            var quote = await finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);
            if (quote != null && profile != null)
            {
                profile.Add("price", quote["c"]);
            }

            if (profile != null && profile.ContainsKey("logo"))
                return View(profile);
            else
                return Content("");
        }
    }
}
