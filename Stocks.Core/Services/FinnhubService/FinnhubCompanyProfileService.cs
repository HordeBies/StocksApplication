using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.ServiceContracts.FinnhubService;

namespace Stocks.Core.Services.FinnhubService
{
    public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
    {
        private readonly IFinnhubRepository finnhubRepository;
        public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository)
        {
            this.finnhubRepository = finnhubRepository;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string symbol)
        {
            var responseDictionary = await finnhubRepository.GetCompanyProfile(symbol);

            return responseDictionary;
        }
    }
}
