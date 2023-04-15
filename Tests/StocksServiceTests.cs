using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using Services;

namespace Tests
{
    public class StocksServiceTests
    {
        private readonly StocksService stocksService;
        public StocksServiceTests()
        {
            stocksService = new StocksService(new StockMarketDbContext(new DbContextOptionsBuilder<StockMarketDbContext>().Options)); //TODO: Unit testing using Moq
        }
        #region CreateBuyOrder
        private static BuyOrderRequest GetValidBuyOrderRequest()
        {
            return new()
            {
                StockName = "Microsoft",
                StockSymbol = "MSFT",
                DateAndTimeOfOrder = new(2001, 1, 1),
                Price = 300.00d,
                Quantity = 15,
            };
        }
        [Fact]
        public async Task CreateBuyOrder_NullRequest()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await stocksService.CreateBuyOrder(null));
        }
        [Fact]
        public async Task CreateBuyOrder_InvalidBuyOrderQuantity_Lower()
        {
            var request = GetValidBuyOrderRequest();
            request.Quantity = 0;
            await Assert.ThrowsAsync<ArgumentException>(async () => await stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public async Task CreateBuyOrder_InvalidBuyOrderQuantity_Upper()
        {
            var request = GetValidBuyOrderRequest();
            request.Quantity = 100001;
            await Assert.ThrowsAsync<ArgumentException>(async () => await stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public async Task CreateBuyOrder_InvalidPrice_Lower()
        {
            var request = GetValidBuyOrderRequest();
            request.Price = 0;
            await Assert.ThrowsAsync<ArgumentException>(async () => await stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public async Task CreateBuyOrder_InvalidPrice_Upper()
        {
            var request = GetValidBuyOrderRequest();
            request.Price = 10001;
            await Assert.ThrowsAsync<ArgumentException>(async () => await stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public async Task CreateBuyOrder_InvalidStockSymbol()
        {
            var request = GetValidBuyOrderRequest();
            request.StockSymbol = null;
            await Assert.ThrowsAsync<ArgumentException>(async() => await stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public async Task CreateBuyOrder_InvalidDateAndTimeOfOrder()
        {
            var request = GetValidBuyOrderRequest();
            request.DateAndTimeOfOrder = new(1999, 12, 31);
            await Assert.ThrowsAsync<ArgumentException>(async() => await stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public async Task CreateBuyOrder_ValidRequest()
        {
            var request = GetValidBuyOrderRequest();
            var expected = await stocksService.CreateBuyOrder(request);
            var collection = await stocksService.GetBuyOrders();
            Assert.True(expected.BuyOrderID != Guid.Empty);
            Assert.Contains(expected, collection);
        }
        #endregion
        #region CreateSellOrder
        private static SellOrderRequest GetValidSellOrderRequest()
        {
            return new()
            {
                StockName = "Microsoft",
                StockSymbol = "MSFT",
                DateAndTimeOfOrder = new(2001, 1, 1),
                Price = 300.00d,
                Quantity = 15,
            };
        }
        [Fact]
        public async Task CreateSellOrder_NullRequest()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async() => await stocksService.CreateSellOrder(null));
        }
        [Fact]
        public async Task CreateSellOrder_InvalidSellOrderQuantity_Lower()
        {
            var request = GetValidSellOrderRequest();
            request.Quantity = 0;
            await Assert.ThrowsAsync<ArgumentException>(async () => await stocksService.CreateSellOrder(request));
        }
        [Fact]
        public async Task CreateSellOrder_InvalidSellOrderQuantity_Upper()
        {
            var request = GetValidSellOrderRequest();
            request.Quantity = 100001;
            await Assert.ThrowsAsync<ArgumentException>(async () => await stocksService.CreateSellOrder(request));
        }
        [Fact]
        public async Task CreateSellOrder_InvalidPrice_Lower()
        {
            var request = GetValidSellOrderRequest();
            request.Price = 0;
            await Assert.ThrowsAsync<ArgumentException>(async() => await stocksService.CreateSellOrder(request));
        }
        [Fact]
        public async Task CreateSellOrder_InvalidPrice_Upper()
        {
            var request = GetValidSellOrderRequest();
            request.Price = 10001;
            await Assert.ThrowsAsync<ArgumentException>(async() => await stocksService.CreateSellOrder(request));
        }
        [Fact]
        public async Task CreateSellOrder_InvalidStockSymbol()
        {
            var request = GetValidSellOrderRequest();
            request.StockSymbol = null;
            await Assert.ThrowsAsync<ArgumentException>(async() => await stocksService.CreateSellOrder(request));
        }
        [Fact]
        public async Task CreateSellOrder_InvalidDateAndTimeOfOrder()
        {
            var request = GetValidSellOrderRequest();
            request.DateAndTimeOfOrder = new(1999, 12, 31);
            await Assert.ThrowsAsync<ArgumentException>(async () => await stocksService.CreateSellOrder(request));
        }
        [Fact]
        public async Task CreateSellOrder_ValidRequestAsync()
        {
            var request = GetValidSellOrderRequest();
            var expected = await stocksService.CreateSellOrder(request);
            var collection = await stocksService.GetSellOrders();
            Assert.True(expected.SellOrderID != Guid.Empty);
            Assert.Contains(expected, collection);
        }
        #endregion
        #region GetBuyOrders
        [Fact]
        public async Task GetBuyOrders_EmptyAsync()
        {
            var collection = await stocksService.GetBuyOrders();
            Assert.NotNull(collection);
            Assert.Empty(collection);
        }
        [Fact]
        public async Task GetBuyOrders_ValidRequestAsync()
        {
            var requests = new BuyOrderRequest[]
            {
                GetValidBuyOrderRequest(),
                GetValidBuyOrderRequest(),
                GetValidBuyOrderRequest(),
                GetValidBuyOrderRequest(),
                GetValidBuyOrderRequest(),
            };
            var expected = await Task.WhenAll(requests.Select(async r => await stocksService.CreateBuyOrder(r)));
            var actual = await stocksService.GetBuyOrders();
            Assert.Equal(expected.Length, actual.Count);
            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }
        }
        #endregion
        #region GetSellOrders
        [Fact]
        public async Task GetSellOrders_EmptyAsync()
        {
            var collection = await stocksService.GetSellOrders();
            Assert.NotNull(collection);
            Assert.Empty(collection);
        }
        [Fact]
        public async Task GetSellOrders_ValidRequestAsync()
        {
            var requests = new SellOrderRequest[]
            {
                GetValidSellOrderRequest(),
                GetValidSellOrderRequest(),
                GetValidSellOrderRequest(),
                GetValidSellOrderRequest(),
                GetValidSellOrderRequest(),
            };
            var expected = await Task.WhenAll(requests.Select(async r => await stocksService.CreateSellOrder(r)));
            var actual = await stocksService.GetSellOrders();
            Assert.Equal(expected.Length, actual.Count);
            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }
        }
        #endregion
    }

}
