using RepositoryContracts;
using ServiceContracts.FinnhubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FinnhubService
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
