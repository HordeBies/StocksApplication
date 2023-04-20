using RepositoryContracts;
using ServiceContracts;
namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository finnhubRepository;
        public FinnhubService(IFinnhubRepository finnhubRepository)
        {
            this.finnhubRepository = finnhubRepository;
        }
        public async Task<Dictionary<string,object>?> GetStockPriceQuote(string symbol)
        {
            var responseDictionary = await finnhubRepository.GetStockPriceQuote(symbol);

            return responseDictionary;
        }
        public async Task<Dictionary<string,object>?> GetCompanyProfile(string symbol)
        {
            var responseDictionary = await finnhubRepository.GetCompanyProfile(symbol);

            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var responseList = await finnhubRepository.GetStocks();

            return responseList;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string symbol)
        {
            var responseDictionary = await finnhubRepository.SearchStocks(symbol);

            return responseDictionary;
        }
    }
}
