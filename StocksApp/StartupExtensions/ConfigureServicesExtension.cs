using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;
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
            services.AddScoped<IFinnhubService, FinnhubService>();
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddScoped<IStocksService, StocksService>();
            services.AddScoped<IStocksRepository, StocksRepository>();
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
