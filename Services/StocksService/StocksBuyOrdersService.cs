using RepositoryContracts;
using ServiceContracts.StocksService;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StocksService
{
    public class StocksBuyOrdersService : IStocksBuyOrdersService
    {
        private readonly IStocksRepository stocksRepository;
        public StocksBuyOrdersService(IStocksRepository stocksRepository)
        {
            this.stocksRepository = stocksRepository;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            var buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            buyOrder = await stocksRepository.CreateBuyOrder(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }
        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return (await stocksRepository.GetBuyOrders()).Select(i => i.ToBuyOrderResponse()).ToList();
        }
    }
}
