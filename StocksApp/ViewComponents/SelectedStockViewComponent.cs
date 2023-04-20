using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace StocksApp.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubService finnhubService;
        public SelectedStockViewComponent(IFinnhubService finnhubService)
        {
            this.finnhubService = finnhubService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
                return Content("");
            var profile = await finnhubService.GetCompanyProfile(stockSymbol);
            var quote = await finnhubService.GetStockPriceQuote(stockSymbol);
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
