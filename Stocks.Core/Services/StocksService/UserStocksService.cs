using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.DTO;
using Stocks.Core.ServiceContracts.StocksService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Core.Services.StocksService
{
    public class UserStocksService : IUserStocksService
    {
        private readonly IUserStocksRepository stocksRepository;
        public UserStocksService(IUserStocksRepository stocksRepository)
        {
            this.stocksRepository = stocksRepository;
        }

        public async Task<UserStock?> GetUserStock(string userId, string stockSymbol)
        {
            return await stocksRepository.GetUserStock(userId, stockSymbol);
        }

        public async Task<List<UserStock>> GetUserStocks(string userId)
        {
            return await stocksRepository.GetUserStocks(userId);
        }

        public async Task<UserStock> UpdateUserStock(BuyOrderRequest request, string userId)
        {
            var userStockFromDb = await stocksRepository.GetUserStock(userId, request.StockSymbol);
            if(userStockFromDb == null)
            {
                var userStock = request.ToUserStock(userId);
                return await stocksRepository.CreateUserStock(userStock);
            }
            else
            {
                userStockFromDb.Amount += (int)request.Quantity;
                userStockFromDb.Cost += request.Quantity * request.Price;
                userStockFromDb.LastChange = request.DateAndTimeOfOrder;
                return await stocksRepository.UpdateUserStock(userStockFromDb);
            }
        }
        public async Task<UserStock> UpdateUserStock(SellOrderRequest request, string userId)
        {
            var userStockFromDb = await stocksRepository.GetUserStock(userId, request.StockSymbol);
            if (userStockFromDb == null)
            {
                throw new Exception("User stock not found");
            }
            else
            {
                userStockFromDb.Amount -= (int)request.Quantity;
                userStockFromDb.Cost -= request.Quantity * request.Price;
                userStockFromDb.LastChange = request.DateAndTimeOfOrder;
                return await stocksRepository.UpdateUserStock(userStockFromDb);
            }
        }
    }
}
