using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.StocksService;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StocksService
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
