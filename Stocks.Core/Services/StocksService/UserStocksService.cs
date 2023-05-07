using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
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
        private readonly IStocksRepository stocksRepository;
        public UserStocksService(IStocksRepository stocksRepository)
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
    }
}
