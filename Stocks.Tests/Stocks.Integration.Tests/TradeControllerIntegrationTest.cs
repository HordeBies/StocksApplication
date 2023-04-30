using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Stocks.Tests.Integration
{
    public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        #region Index

        [Fact]
        public async Task Index_ToReturnView()
        {

            HttpResponseMessage response = await _client.GetAsync("/Trade/Index/MSFT");

            response.Should().BeSuccessful();

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);
            var document = html.DocumentNode;

            document.QuerySelectorAll(".price").Should().NotBeNull();
        }

        #endregion
    }
}
