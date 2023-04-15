using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IStocksService
    {
        /// <summary>
        /// Creates a buy order with the specified buy order request and returns the buy order response.
        /// </summary>
        /// <param name="buyOrderRequest">The buy order request to create the buy order.</param>
        /// <returns>The buy order response created by the system.</returns>
        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        /// <summary>
        /// Creates a sell order with the specified sell order request and returns the sell order response.
        /// </summary>
        /// <param name="sellOrderRequest">The sell order request to create the sell order.</param>
        /// <returns>The sell order response created by the system.</returns>
        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        /// <summary>
        /// Returns a list of buy order responses that includes all buy orders created by the system.
        /// </summary>
        /// <returns>A list of buy order responses that includes all buy orders created by the system.</returns>
        public Task<List<BuyOrderResponse>> GetBuyOrders();

        /// <summary>
        /// Returns a list of sell order responses that includes all sell orders created by the system.
        /// </summary>
        /// <returns>A list of sell order responses that includes all sell orders created by the system.</returns>
        public Task<List<SellOrderResponse>> GetSellOrders();

    }
}
