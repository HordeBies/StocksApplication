using StocksApp.ServiceContracts;
using System.Text.Json;
namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }
        public async Task<Dictionary<string,object>> GetQuote(string symbol)
        {
            using(var httpClient = httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={symbol}&token={configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                string response = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if(responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from finnhub server");
                }else if (responseDictionary.TryGetValue("error", out object value))
                {
                    throw new InvalidOperationException(value.ToString());
                }
                return responseDictionary;
            }
        }
        public async Task<Dictionary<string,object>> GetCompanyProfile(string symbol)
        {
            using(var httpClient = httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                string response = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();
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
