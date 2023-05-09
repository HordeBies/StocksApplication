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
    public class UserRepository : IUserRepository
    {
        private readonly StockMarketDbContext db;
        public UserRepository(StockMarketDbContext stockMarketDbContext)
        {
            db = stockMarketDbContext;
        }
        public async Task<ApplicationUser> UpdateBalance(ApplicationUser user)
        {
            var userFromDb = await db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            userFromDb.Balance = user.Balance;
            db.Users.Update(userFromDb);
            await db.SaveChangesAsync();
            return userFromDb;
        }
        public async Task<ApplicationUser> GetUser(string userId)
        {
            return await db.Users.FirstOrDefaultAsync(r => r.Id == userId);
        }
    }
}
