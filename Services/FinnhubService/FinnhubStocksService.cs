using RepositoryContracts;
using ServiceContracts.FinnhubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FinnhubService
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
