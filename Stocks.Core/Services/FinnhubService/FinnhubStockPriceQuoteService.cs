using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.ServiceContracts.FinnhubService;

namespace Stocks.Core.Services.FinnhubService
{
    public class FinnhubStockPriceQuoteService : IFinnhubStockPriceQuoteService
    {
        private readonly IFinnhubRepository finnhubRepository;
        public FinnhubStockPriceQuoteService(IFinnhubRepository finnhubRepository)
        {
            this.finnhubRepository = finnhubRepository;
        }
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string symbol)
        {
            var responseDictionary = await finnhubRepository.GetStockPriceQuote(symbol);

            return responseDictionary;
        }
    }
}
