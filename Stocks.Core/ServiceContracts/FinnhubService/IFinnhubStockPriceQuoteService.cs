
namespace Stocks.Core.ServiceContracts.FinnhubService
{
    public interface IFinnhubStockPriceQuoteService
    {
        /// <summary>
        /// Returns a dictionary of stock price quote information for the given stock symbol.
        /// </summary>
        /// <param name="symbol">The stock symbol for which to retrieve stock price quote information.</param>
        /// <returns>A dictionary of stock price quote information for the given stock symbol.</returns>
        public Task<Dictionary<string, object>?> GetStockPriceQuote(string symbol);
    }
}
