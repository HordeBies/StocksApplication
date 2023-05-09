using Stocks.Core.Domain.Entities;

namespace Stocks.Core.Domain.RepositoryContracts
{
    public interface IStockOrdersRepository
    {
        /// <summary>
        /// Creates a new buy order for a stock.
        /// </summary>
        /// <param name="buyOrder">The buy order to create.</param>
        /// <returns>The created buy order.</returns>
        public Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        /// <summary>
        /// Creates a new sell order for a stock.
        /// </summary>
        /// <param name="sellOrder">The sell order to create.</param>
        /// <returns>The created sell order.</returns>
        public Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        /// <summary>
        /// Retrieves a list of all buy orders for stocks.
        /// </summary>
        /// <returns>A list of all buy orders for stocks.</returns>
        public Task<List<BuyOrder>> GetBuyOrders(string userId);

        /// <summary>
        /// Retrieves a list of all sell orders for stocks.
        /// </summary>
        /// <returns>A list of all sell orders for stocks.</returns>
        public Task<List<SellOrder>> GetSellOrders(string userId);
    }
}