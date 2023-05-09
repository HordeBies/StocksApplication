using Microsoft.EntityFrameworkCore;
using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Infrastructure.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Infrastructure.Repositories
{
    public class UserStocksRepository : IUserStocksRepository
    {
        private readonly StockMarketDbContext db;
        public UserStocksRepository(StockMarketDbContext stockMarketDbContext)
        {
            db = stockMarketDbContext;
        }

        public async Task<UserStock> CreateUserStock(UserStock userStock)
        {
            db.UserStocks.Add(userStock);
            await db.SaveChangesAsync();
            return userStock;
        }
        public async Task<UserStock> UpdateUserStock(UserStock userStock)
        {
            var userStockFromDb = await db.UserStocks.Where(r => r.ApplicationUserId == userStock.ApplicationUserId && r.StockId == userStock.StockId).FirstOrDefaultAsync();
            userStockFromDb.Cost = userStock.Cost;
            userStockFromDb.LastChange = userStock.LastChange;
            userStockFromDb.Amount = userStock.Amount;
            db.UserStocks.Update(userStockFromDb);
            await db.SaveChangesAsync();
            return userStock;
        }

        public async Task<UserStock?> GetUserStock(string userId, string stockSymbol)
        {
            return await db.UserStocks.Where(r => r.ApplicationUserId == userId && r.StockId == stockSymbol).FirstOrDefaultAsync();
        }
        public async Task<List<UserStock>> GetUserStocks(string userId)
        {
            return await db.UserStocks.Where(r => r.ApplicationUserId == userId).ToListAsync();
        }

    }
}
