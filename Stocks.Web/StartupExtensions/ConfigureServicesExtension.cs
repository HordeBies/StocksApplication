using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.ServiceContracts;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Core.ServiceContracts.StocksService;
using Stocks.Core.Services;
using Stocks.Core.Services.FinnhubService;
using Stocks.Core.Services.StocksService;
using Stocks.Infrastructure.DatabaseContext;
using Stocks.Infrastructure.DatabaseInitializers;
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
            services.AddRazorPages();
            services.AddHttpClient();
            services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));

            //Custom Filters
            services.AddTransient<CreateOrderActionFilter>();

            //Custom Services
            services.AddScoped<IStockMarketDbInitializer, StockMarketDbInitializer>();
            //Finnhub Services
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
            services.AddScoped<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
            services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
            services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();
            //Stocks Services
            services.AddScoped<IStockOrdersRepository, StockOrdersRepository>();
            services.AddScoped<IStocksBuyOrdersService, StocksBuyOrdersService>();
            services.AddScoped<IStocksSellOrdersService, StocksSellOrdersService>();
            services.AddScoped<IUserStocksRepository, UserStocksRepository>();
            services.AddScoped<IUserStocksService, UserStocksService>();
            //Identity Services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSender, EmailSenderService>();

            services.AddDbContext<StockMarketDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("StockMarketConnection"));
            });
            
            services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<StockMarketDbContext>().AddDefaultTokenProviders().AddDefaultUI();

            var gitHubOptions = configuration.GetSection("Authentication:GitHub").Get<ExternalAuthenticationSettings>();
            var googleOptions = configuration.GetSection("Authentication:Google").Get<ExternalAuthenticationSettings>();
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = googleOptions.ClientId;
                options.ClientSecret = googleOptions.ClientSecret;
            }).AddGitHub(options =>
            {
                options.ClientId = gitHubOptions.ClientId;
                options.ClientSecret = gitHubOptions.ClientSecret;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }
    }
}
