using Stocks.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Core.ServiceContracts.StocksService
{
    public interface IUserStocksService
    {
        public Task<List<UserStock>> GetUserStocks(string userId);
        public Task<UserStock?> GetUserStock(string userId, string stockSymbol);
    }
}
