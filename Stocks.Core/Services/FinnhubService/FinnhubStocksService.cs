using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.ServiceContracts.FinnhubService;

namespace Stocks.Core.Services.FinnhubService
{
    public class FinnhubStocksService : IFinnhubStocksService
    {
        private readonly IFinnhubRepository finnhubRepository;
        public FinnhubStocksService(IFinnhubRepository finnhubRepository)
        {
            this.finnhubRepository = finnhubRepository;
        }
        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var responseList = await finnhubRepository.GetStocks();

            return responseList;
        }
    }
}
