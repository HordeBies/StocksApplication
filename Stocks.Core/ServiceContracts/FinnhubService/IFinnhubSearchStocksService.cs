﻿
namespace Stocks.Core.ServiceContracts.FinnhubService
{
    public interface IFinnhubSearchStocksService
    {
        /// <summary>
        /// Returns a dictionary of stock information that matches the given stock symbol to search for.
        /// </summary>
        /// <param name="symbol">The stock symbol to search for.</param>
        /// <returns>A dictionary of stock information that matches the given stock symbol to search for.</returns>
        public Task<Dictionary<string, object>?> SearchStocks(string symbol);
    }
}
