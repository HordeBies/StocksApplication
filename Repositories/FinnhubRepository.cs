using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using System.Text.Json;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly ILogger<FinnhubRepository> logger;
        private readonly IDiagnosticContext diagnostic;
        public FinnhubRepository(ILogger<FinnhubRepository> logger, IDiagnosticContext diagnosticContext ,IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.logger = logger;
            this.diagnostic = diagnosticContext;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string symbol)
        {
            //Log
            logger.LogInformation("In {ClassName}.{MethodName}", nameof(FinnhubRepository), nameof(GetStockPriceQuote));

            using (var httpClient = httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={symbol}&token={configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                diagnostic.Set("Finnhub Response", response);
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from finnhub server");
                }
                else if (responseDictionary.TryGetValue("error", out object value))
                {
                    throw new InvalidOperationException(value.ToString());
                }
                return responseDictionary;
            }
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string symbol)
        {
            //Log
            logger.LogInformation("In {ClassName}.{MethodName}", nameof(FinnhubRepository), nameof(GetCompanyProfile));

            using (var httpClient = httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                diagnostic.Set("Finnhub Response", response);
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from finnhub server");
                }
                else if (responseDictionary.TryGetValue("error", out object value))
                {
                    throw new InvalidOperationException(value.ToString());
                }
                return responseDictionary;
            }
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            //Log
            logger.LogInformation("In {ClassName}.{MethodName}", nameof(FinnhubRepository), nameof(GetStocks));

            using (var httpClient = httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                //diagnostic.Set("Finnhub Response", response);
                var responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(response);
                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from finnhub server");
                }
                return responseDictionary;
            }
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string symbol)
        {
            //Log
            logger.LogInformation("In {ClassName}.{MethodName}", nameof(FinnhubRepository), nameof(SearchStocks));

            using (var httpClient = httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={symbol}&token={configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                diagnostic.Set("Finnhub Response", response);
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from finnhub server");
                }
                else if (responseDictionary.TryGetValue("error", out object value))
                {
                    throw new InvalidOperationException(value.ToString());
                }
                return responseDictionary;
            }
        }
    }
}
