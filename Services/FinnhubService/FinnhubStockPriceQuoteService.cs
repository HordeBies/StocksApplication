using RepositoryContracts;
using ServiceContracts.FinnhubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FinnhubService
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
