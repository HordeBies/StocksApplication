using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Infrastructure.DatabaseContext;

namespace Stocks.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            var secretsBuilder = new ConfigurationBuilder().AddUserSecrets<TradeControllerIntegrationTest>();
            IConfiguration secrets = secretsBuilder.Build();

            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var descripter = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<StockMarketDbContext>));

                if (descripter != null)
                {
                    services.Remove(descripter);
                }
                services.AddDbContext<StockMarketDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatbaseForTesting");
                });
            });
            builder.ConfigureAppConfiguration((WebHostBuilderContext ctx, IConfigurationBuilder config) =>
            {
                var newConfiguration = new Dictionary<string, string>() { { "FinnhubToken", secrets["FinnhubToken"] } };

                config.AddInMemoryCollection(newConfiguration!);
            });

        }
    }
}
