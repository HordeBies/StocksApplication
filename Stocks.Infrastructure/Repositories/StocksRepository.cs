using Microsoft.EntityFrameworkCore;
using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Infrastructure.DatabaseContext;

namespace Stocks.Infrastructure.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly StockMarketDbContext db;
        public StocksRepository(StockMarketDbContext stockMarketDbContext)
        {
            db = stockMarketDbContext;
        }
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            db.BuyOrders.Add(buyOrder);
            // TODO: Add User Stock Repository to handle this and call it in stock service
            var userStock = db.UserStocks.Where(r => r.ApplicationUserId == buyOrder.UserId && r.StockId == buyOrder.StockSymbol).FirstOrDefault();
            if(userStock != null)
            {
                userStock.Amount += (int)buyOrder.Quantity;
                db.UserStocks.Update(userStock);
            }
            else
            {
                db.UserStocks.Add(new UserStock() { ApplicationUserId = buyOrder.UserId, StockId = buyOrder.StockSymbol, Amount = (int)buyOrder.Quantity });
            }
            // TODO: Add User Repository to handle this and call it in stock service
            var user = db.Users.Where(r => r.Id == buyOrder.UserId).FirstOrDefault();
            user.Balance -= buyOrder.Price * buyOrder.Quantity;
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            db.SellOrders.Add(sellOrder);
            // TODO: Add User Stock Repository to handle this and call it in stock service
            var userStock = db.UserStocks.Where(r => r.ApplicationUserId == sellOrder.UserId && r.StockId == sellOrder.StockSymbol).FirstOrDefault();
            if (userStock != null)
            {
                userStock.Amount -= (int)sellOrder.Quantity;
                db.UserStocks.Update(userStock);
            }
            else
            {
                db.UserStocks.Add(new UserStock() { ApplicationUserId = sellOrder.UserId, StockId = sellOrder.StockSymbol, Amount = (int)sellOrder.Quantity });
            }
            // TODO: Add User Repository to handle this and call it in stock service
            var user = db.Users.Where(r => r.Id == sellOrder.UserId).FirstOrDefault();
            user.Balance += sellOrder.Price * sellOrder.Quantity;
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders(string userId)
        {
            return await db.BuyOrders.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<List<SellOrder>> GetSellOrders(string userId)
        {
            return await db.SellOrders.Where(r => r.UserId == userId).ToListAsync();
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