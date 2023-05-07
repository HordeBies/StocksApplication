using Stocks.Core.DTO;

namespace Stocks.Core.ServiceContracts.StocksService
{
    public interface IStocksSellOrdersService
    {
        /// <summary>
        /// Creates a sell order with the specified sell order request and returns the sell order response.
        /// </summary>
        /// <param name="sellOrderRequest">The sell order request to create the sell order.</param>
        /// <returns>The sell order response created by the system.</returns>
        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest, string userId);

        /// <summary>
        /// Returns a list of sell order responses that includes all sell orders created by the system.
        /// </summary>
        /// <returns>A list of sell order responses that includes all sell orders created by the system.</returns>
        public Task<List<SellOrderResponse>> GetSellOrders(string userId);
    }
}
