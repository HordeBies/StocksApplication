using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stocks.Core.Domain.Entities;

namespace Stocks.Infrastructure.DatabaseContext
{
    public class StockMarketDbContext : IdentityDbContext<ApplicationUser>
    {
        public StockMarketDbContext(DbContextOptions<StockMarketDbContext> options) : base(options)
        {
        }

        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }
        public DbSet<UserStock> UserStocks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");
            modelBuilder.Entity<ApplicationUser>().Property(r => r.Balance).HasDefaultValue(10000);

            modelBuilder.Entity<UserStock>().HasKey(us => new { us.ApplicationUserId, us.StockId });
            modelBuilder.Entity<UserStock>().Property(r => r.Amount).HasDefaultValue(0);
        }
    }
}
