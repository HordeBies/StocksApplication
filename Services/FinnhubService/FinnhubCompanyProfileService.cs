using RepositoryContracts;
using ServiceContracts.FinnhubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FinnhubService
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
