using Microsoft.EntityFrameworkCore;
using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Infrastructure.DatabaseContext;

namespace Stocks.Infrastructure.Repositories
{
    public class StockOrdersRepository : IStockOrdersRepository
    {
        private readonly StockMarketDbContext db;
        public StockOrdersRepository(StockMarketDbContext stockMarketDbContext)
        {
            db = stockMarketDbContext;
        }
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            db.BuyOrders.Add(buyOrder);
            await db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            db.SellOrders.Add(sellOrder);
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
    }
}