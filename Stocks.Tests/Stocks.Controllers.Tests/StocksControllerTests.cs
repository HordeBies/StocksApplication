﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks.Core.ServiceContracts.FinnhubService;
using Stocks.Web.Areas.User.Models;
using Stocks.Web.Configurations;
using Stocks.Web.Areas.User.Controllers;

namespace Stocks.Tests.Controllers
{
    public class StocksControllerTests
    {
        private readonly IFinnhubStocksService finnhubStocksService;
        private readonly Mock<IFinnhubStocksService> finnhubStocksServiceMock;
        public StocksControllerTests()
        {
            finnhubStocksServiceMock = new();
            finnhubStocksService = finnhubStocksServiceMock.Object;
        }
        #region Explore
        [Fact]
        public async Task Explore_NullStock()
        {
            IOptions<TradingOptions> tradingOptions = Options.Create(new TradingOptions() { DefaultOrderQuantity = 100, Top25PopularStocks = new() { "AAPL", "MSFT", "AMZN", "TSLA", "GOOGL", "GOOG", "NVDA", "BRK.B", "META", "UNH", "JNJ", "JPM", "V", "PG", "XOM", "HD", "CVX", "MA", "BAC", "ABBV", "PFE", "AVGO", "COST", "DIS", "KO" } });

            var controller = new StocksController(finnhubStocksService, tradingOptions);

            List<Dictionary<string, string>> stocksDict = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(@"[{'currency':'USD','description':'APPLE INC','displaySymbol':'AAPL','figi':'BBG000B9XRY4','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5N8V8','symbol':'AAPL','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'MICROSOFT CORP','displaySymbol':'MSFT','figi':'BBG000BPH459','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5TD05','symbol':'MSFT','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'AMAZON.COM INC','displaySymbol':'AMZN','figi':'BBG000BVPV84','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5PQL7','symbol':'AMZN','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'TESLA INC','displaySymbol':'TSLA','figi':'BBG000N9MNX3','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001SQKGD7','symbol':'TSLA','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'ALPHABET INC-CL A','displaySymbol':'GOOGL','figi':'BBG009S39JX6','isin':null,'mic':'XNAS','shareClassFIGI':'BBG009S39JY5','symbol':'GOOGL','symbol2':'','type':'Common Stock'}]");

            finnhubStocksServiceMock.Setup(temp => temp.GetStocks()).ReturnsAsync(stocksDict);

            var expectedStocks = stocksDict.Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) }).ToList();

            var result = await controller.Explore();

            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<Stock>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(expectedStocks);


        }
        #endregion
    }
}
