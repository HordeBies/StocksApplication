﻿using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
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
            await db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            db.SellOrders.Add(sellOrder);
            await db.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            return await db.BuyOrders.ToListAsync();
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            return await db.SellOrders.ToListAsync();
        }
    }
}