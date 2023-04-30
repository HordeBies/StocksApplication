using Microsoft.EntityFrameworkCore;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Core.ServiceContracts.StocksService;
using Stocks.Core.Services.FinnhubService;
using Stocks.Core.Services.StocksService;
using Stocks.Infrastructure.DatabaseContext;
using Stocks.Infrastructure.Repositories;
using Stocks.Web.Configurations;
using Stocks.Web.Filters.ActionFilters;

namespace Stocks.Web.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));

            //Custom Filters
            services.AddTransient<CreateOrderActionFilter>();

            //Custom Services
            //Finnhub Services
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
            services.AddScoped<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
            services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
            services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();
            //Stocks Services
            services.AddScoped<IStocksRepository, StocksRepository>();
            services.AddScoped<IStocksBuyOrdersService, StocksBuyOrdersService>();
            services.AddScoped<IStocksSellOrdersService, StocksSellOrdersService>();

            services.AddDbContext<StockMarketDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("StockMarketConnection"));
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }
    }
}
