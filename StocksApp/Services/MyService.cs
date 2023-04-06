namespace StocksApp.Services
{
    public class MyService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public MyService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task Method()
        {
            using(var httpClient = httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri("url"),
                    Method = HttpMethod.Get,
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            }
        }
    }
}
