using ServiceContracts.DTO;
using Services;

namespace Tests
{
    public class StocksServiceTests
    {
        private readonly StocksService stocksService;
        public StocksServiceTests()
        {
            stocksService = new();
        }
        #region CreateBuyOrder
        private BuyOrderRequest GetValidBuyOrderRequest()
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
        public void CreateBuyOrder_NullRequest()
        {
            Assert.Throws<ArgumentNullException>(()=>stocksService.CreateBuyOrder(null));
        }
        [Fact]
        public void CreateBuyOrder_InvalidBuyOrderQuantity_Lower()
        {
            var request = GetValidBuyOrderRequest();
            request.Quantity = 0;
            Assert.Throws<ArgumentException>(()=>stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public void CreateBuyOrder_InvalidBuyOrderQuantity_Upper()
        {
            var request = GetValidBuyOrderRequest();
            request.Quantity = 100001;
            Assert.Throws<ArgumentException>(() => stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public void CreateBuyOrder_InvalidPrice_Lower()
        {
            var request = GetValidBuyOrderRequest();
            request.Price = 0;
            Assert.Throws<ArgumentException>(() => stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public void CreateBuyOrder_InvalidPrice_Upper()
        {
            var request = GetValidBuyOrderRequest();
            request.Price = 10001;
            Assert.Throws<ArgumentException>(() => stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public void CreateBuyOrder_InvalidStockSymbol()
        {
            var request = GetValidBuyOrderRequest();
            request.StockSymbol = null;
            Assert.Throws<ArgumentException>(() => stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public void CreateBuyOrder_InvalidDateAndTimeOfOrder()
        {
            var request = GetValidBuyOrderRequest();
            request.DateAndTimeOfOrder = new(1999, 12, 31);
            Assert.Throws<ArgumentException>(() => stocksService.CreateBuyOrder(request));
        }
        [Fact]
        public void CreateBuyOrder_ValidRequest()
        {
            var request = GetValidBuyOrderRequest();
            var expected = stocksService.CreateBuyOrder(request);
            var collection = stocksService.GetBuyOrders();
            Assert.True(expected.BuyOrderID != Guid.Empty);
            Assert.Contains(expected, collection);
        }
        #endregion
        #region CreateSellOrder
        [Fact]
        private SellOrderRequest GetValidSellOrderRequest()
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
        public void CreateSellOrder_NullRequest()
        {
            Assert.Throws<ArgumentNullException>(() => stocksService.CreateSellOrder(null));
        }
        [Fact]
        public void CreateSellOrder_InvalidSellOrderQuantity_Lower()
        {
            var request = GetValidSellOrderRequest();
            request.Quantity = 0;
            Assert.Throws<ArgumentException>(() => stocksService.CreateSellOrder(request));
        }
        [Fact]
        public void CreateSellOrder_InvalidSellOrderQuantity_Upper()
        {
            var request = GetValidSellOrderRequest();
            request.Quantity = 100001;
            Assert.Throws<ArgumentException>(() => stocksService.CreateSellOrder(request));
        }
        [Fact]
        public void CreateSellOrder_InvalidPrice_Lower()
        {
            var request = GetValidSellOrderRequest();
            request.Price = 0;
            Assert.Throws<ArgumentException>(() => stocksService.CreateSellOrder(request));
        }
        [Fact]
        public void CreateSellOrder_InvalidPrice_Upper()
        {
            var request = GetValidSellOrderRequest();
            request.Price = 10001;
            Assert.Throws<ArgumentException>(() => stocksService.CreateSellOrder(request));
        }
        [Fact]
        public void CreateSellOrder_InvalidStockSymbol()
        {
            var request = GetValidSellOrderRequest();
            request.StockSymbol = null;
            Assert.Throws<ArgumentException>(() => stocksService.CreateSellOrder(request));
        }
        [Fact]
        public void CreateSellOrder_InvalidDateAndTimeOfOrder()
        {
            var request = GetValidSellOrderRequest();
            request.DateAndTimeOfOrder = new(1999, 12, 31);
            Assert.Throws<ArgumentException>(() => stocksService.CreateSellOrder(request));
        }
        [Fact]
        public void CreateSellOrder_ValidRequest()
        {
            var request = GetValidSellOrderRequest();
            var expected = stocksService.CreateSellOrder(request);
            var collection = stocksService.GetSellOrders();
            Assert.True(expected.SellOrderID != Guid.Empty);
            Assert.Contains(expected, collection);
        }
        #endregion
        #region GetBuyOrders
        [Fact]
        public void GetBuyOrders_Empty()
        {
            var collection = stocksService.GetBuyOrders();
            Assert.NotNull(collection);
            Assert.Empty(collection);
        }
        [Fact]
        public void GetBuyOrders_ValidRequest()
        {
            var requests = new BuyOrderRequest[]
            {
                GetValidBuyOrderRequest(),
                GetValidBuyOrderRequest(),
                GetValidBuyOrderRequest(),
                GetValidBuyOrderRequest(),
                GetValidBuyOrderRequest(),
            };
            var expected = requests.Select(r => stocksService.CreateBuyOrder(r)).ToList();
            var actual = stocksService.GetBuyOrders();
            Assert.Equal(expected.Count, actual.Count);
            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }
        }
        #endregion
        #region GetSellOrders
        [Fact]
        public void GetSellOrders_Empty()
        {
            var collection = stocksService.GetSellOrders();
            Assert.NotNull(collection);
            Assert.Empty(collection);
        }
        [Fact]
        public void GetSellOrders_ValidRequest()
        {
            var requests = new SellOrderRequest[]
            {
                GetValidSellOrderRequest(),
                GetValidSellOrderRequest(),
                GetValidSellOrderRequest(),
                GetValidSellOrderRequest(),
                GetValidSellOrderRequest(),
            };
            var expected = requests.Select(r => stocksService.CreateSellOrder(r)).ToList();
            var actual = stocksService.GetSellOrders();
            Assert.Equal(expected.Count, actual.Count);
            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }
        }
        #endregion
    }

}
