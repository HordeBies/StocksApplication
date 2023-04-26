using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using Services;
using Services.FinnhubService;
using Services.StocksService;
using StocksApp.Filters.ActionFilters;

namespace StocksApp.StartupExtensions
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
