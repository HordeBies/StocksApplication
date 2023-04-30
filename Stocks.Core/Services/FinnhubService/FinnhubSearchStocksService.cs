using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.ServiceContracts.FinnhubService;

namespace Stocks.Core.Services.FinnhubService
{
    public class FinnhubSearchStocksService : IFinnhubSearchStocksService
    {
        private readonly IFinnhubRepository finnhubRepository;
        public FinnhubSearchStocksService(IFinnhubRepository finnhubRepository)
        {
            this.finnhubRepository = finnhubRepository;
        }
        public async Task<Dictionary<string, object>?> SearchStocks(string symbol)
        {
            var responseDictionary = await finnhubRepository.SearchStocks(symbol);

            return responseDictionary;
        }
    }
}
