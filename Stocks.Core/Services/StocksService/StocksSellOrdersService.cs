using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.DTO;
using Stocks.Core.Helpers;
using Stocks.Core.ServiceContracts.StocksService;

namespace Stocks.Core.Services.StocksService
{
    public class StocksSellOrdersService : IStocksSellOrdersService
    {
        private readonly IStocksRepository stocksRepository;
        public StocksSellOrdersService(IStocksRepository stocksRepository)
        {
            this.stocksRepository = stocksRepository;
        }
        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            var sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            sellOrder = await stocksRepository.CreateSellOrder(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }
        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            return (await stocksRepository.GetSellOrders()).Select(i => i.ToSellOrderResponse()).ToList();
        }
    }
}
