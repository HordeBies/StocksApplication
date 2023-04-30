
namespace Stocks.Core.ServiceContracts.FinnhubService
{
    public interface IFinnhubStocksService
    {
        /// <summary>
        /// Returns a list of dictionaries containing information about all available stocks.
        /// </summary>
        /// <returns>A list of dictionaries containing information about all available stocks.</returns>
        public Task<List<Dictionary<string, string>>?> GetStocks();
    }
}
