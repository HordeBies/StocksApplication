namespace ServiceContracts
{
    public interface IFinnhubService
    {
        /// <summary>
        /// Returns a dictionary of stock price quote information for the given stock symbol.
        /// </summary>
        /// <param name="symbol">The stock symbol for which to retrieve stock price quote information.</param>
        /// <returns>A dictionary of stock price quote information for the given stock symbol.</returns>
        public Task<Dictionary<string, object>?> GetStockPriceQuote(string symbol);

        /// <summary>
        /// Returns a dictionary of company profile information for the given stock symbol.
        /// </summary>
        /// <param name="symbol">The stock symbol for which to retrieve company profile information.</param>
        /// <returns>A dictionary of company profile information for the given stock symbol.</returns>
        public Task<Dictionary<string, object>?> GetCompanyProfile(string symbol);

        /// <summary>
        /// Returns a list of dictionaries containing information about all available stocks.
        /// </summary>
        /// <returns>A list of dictionaries containing information about all available stocks.</returns>
        public Task<List<Dictionary<string, string>>?> GetStocks();

        /// <summary>
        /// Returns a dictionary of stock information that matches the given stock symbol to search for.
        /// </summary>
        /// <param name="symbol">The stock symbol to search for.</param>
        /// <returns>A dictionary of stock information that matches the given stock symbol to search for.</returns>
        public Task<Dictionary<string, object>?> SearchStocks(string symbol);
    }
}
