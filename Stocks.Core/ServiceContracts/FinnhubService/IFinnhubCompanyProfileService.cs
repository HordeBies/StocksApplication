
namespace Stocks.Core.ServiceContracts.FinnhubService
{
    public interface IFinnhubCompanyProfileService
    {
        /// <summary>
        /// Returns a dictionary of company profile information for the given stock symbol.
        /// </summary>
        /// <param name="symbol">The stock symbol for which to retrieve company profile information.</param>
        /// <returns>A dictionary of company profile information for the given stock symbol.</returns>
        public Task<Dictionary<string, object>?> GetCompanyProfile(string symbol);
    }
}
