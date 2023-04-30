namespace Stocks.Core.Domain.RepositoryContracts
{
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Retrieves company profile information for the given stock symbol from Finnhub API.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol for which to retrieve company profile information.</param>
        /// <returns>A dictionary of company profile information for the given stock symbol.</returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        /// <summary>
        /// Retrieves stock price quote information for the given stock symbol from Finnhub API.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol for which to retrieve stock price quote information.</param>
        /// <returns>A dictionary of stock price quote information for the given stock symbol.</returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

        /// <summary>
        /// Retrieves a list of available stocks from Finnhub API.
        /// </summary>
        /// <returns>A list of available stocks.</returns>
        Task<List<Dictionary<string, string>>?> GetStocks();

        /// <summary>
        /// Searches for the given stock symbol in the list of available stocks from Finnhub API.
        /// </summary>
        /// <param name="stockSymbolToSearch">The stock symbol to search for.</param>
        /// <returns>A dictionary of stock information for the given stock symbol.</returns>
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);

    }
}
