using Stocks.Core.DTO;

namespace Stocks.Core.ServiceContracts.StocksService
{
    public interface IStocksBuyOrdersService
    {
        /// <summary>
        /// Creates a buy order with the specified buy order request and returns the buy order response.
        /// </summary>
        /// <param name="buyOrderRequest">The buy order request to create the buy order.</param>
        /// <returns>The buy order response created by the system.</returns>
        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest, string userId);

        /// <summary>
        /// Returns a list of buy order responses that includes all buy orders created by the system.
        /// </summary>
        /// <returns>A list of buy order responses that includes all buy orders created by the system.</returns>
        public Task<List<BuyOrderResponse>> GetBuyOrders(string userId);
    }
}
