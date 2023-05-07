using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.Enums;
using Stocks.Infrastructure.DatabaseContext;

namespace Stocks.Infrastructure.DatabaseInitializers
{
    public class StockMarketDbInitializer : IStockMarketDbInitializer
    {
        private readonly IConfigurationSection configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly StockMarketDbContext db;
        public StockMarketDbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, StockMarketDbContext db, IConfiguration configuration)
        {
            this.configuration = configuration.GetRequiredSection("DbInitializer");
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
        }
        public async Task Initialize()
        {
            try
            {
                if (db.Database.GetPendingMigrations().Count() > 0)
                {
                    db.Database.Migrate();
                }
            }
            catch (Exception e)
            {

            }
            if (!await roleManager.RoleExistsAsync(Role.User))
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Admin));
                await roleManager.CreateAsync(new IdentityRole(Role.User));

                await userManager.CreateAsync(new ApplicationUser
                {
                    FullName = "Mehmet Demirci",
                    UserName = "oa.mehmetdmrc@gmail.com",
                    Email = "oa.mehmetdmrc@gmail.com",
                    PhoneNumber = "5436036810",
                    Balance = 10000000,
                }, configuration["AdminPassword"]);

                var user = await db.Users.FirstOrDefaultAsync(r => r.Email == "oa.mehmetdmrc@gmail.com");
                await userManager.AddToRoleAsync(user, Role.Admin);
            }
        }
    }
}
