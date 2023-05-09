using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.DTO;
using Stocks.Core.Services.StocksService;

namespace Stocks.Tests.Services
{
    public class StocksServiceTests
    {
        private readonly IFixture fixture;

        private readonly StocksBuyOrdersService stocksBuyOrdersService;
        private readonly StocksSellOrdersService stocksSellOrdersService;

        private readonly Mock<IStockOrdersRepository> mock;
        private readonly IStockOrdersRepository repository;

        public StocksServiceTests()
        {
            fixture = new Fixture();
            mock = new();
            repository = mock.Object;

            stocksBuyOrdersService = new(repository);
            stocksSellOrdersService = new(repository);
        }
        #region CreateBuyOrder
        [Fact]
        public async Task CreateBuyOrder_NullBuyOrder()
        {
            var action = (async () => await stocksBuyOrdersService.CreateBuyOrder(null,""));
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_QuantityIsLessThanMinimum(uint buyOrderQuantity)
        {
            var request = fixture.Build<BuyOrderRequest>()
                .With(r => r.Quantity, buyOrderQuantity)
                .Create();
            var action = (async () => await stocksBuyOrdersService.CreateBuyOrder(request,""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Theory]
        [InlineData(100001)]
        public async Task CreateBuyOrder_QuantityMoreThanMaximum(uint buyOrderQuantity)
        {
            var request = fixture.Build<BuyOrderRequest>()
                .With(r => r.Quantity, buyOrderQuantity)
                .Create();
            var action = (async () => await stocksBuyOrdersService.CreateBuyOrder(request, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_PriceIsLessThanMinimum(uint buyOrderPrice)
        {
            var request = fixture.Build<BuyOrderRequest>()
                .With(r => r.Price, buyOrderPrice)
                .Create();
            var action = (async () => await stocksBuyOrdersService.CreateBuyOrder(request,""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Theory]
        [InlineData(10001)]
        public async Task CreateBuyOrder_PriceMoreThanMaximum(uint buyOrderPrice)
        {
            var request = fixture.Build<BuyOrderRequest>()
                .With(r => r.Price, buyOrderPrice)
                .Create();
            var action = (async () => await stocksBuyOrdersService.CreateBuyOrder(request, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull()
        {
            var request = fixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();
            var action = (async () => await stocksBuyOrdersService.CreateBuyOrder(request, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateBuyOrder_DateOfOrderIsLessThanYear2000()
        {
            var request = fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(1999, 12, 31))
                .Create();
            var action = (async () => await stocksBuyOrdersService.CreateBuyOrder(request, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateBuyOrder_ValidRequest()
        {
            var request = fixture.Create<BuyOrderRequest>();
            var order = request.ToBuyOrder();
            mock.Setup(r => r.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(order);
            var expected = order.ToBuyOrderResponse();

            var actual = await stocksBuyOrdersService.CreateBuyOrder(request, "");
            expected.BuyOrderID = actual.BuyOrderID;

            actual.BuyOrderID.Should().NotBeEmpty();
            actual.Should().Be(expected);

        }
        #endregion
        #region CreateSellOrder
        [Fact]
        public async Task CreateSellOrder_NullSellOrder()
        {
            var action = (async () => await stocksSellOrdersService.CreateSellOrder(null, ""));
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_QuantityIsLessThanMinimum(uint sellOrderQuantity)
        {
            var request = fixture.Build<SellOrderRequest>()
                .With(r => r.Quantity, sellOrderQuantity)
                .Create();
            var action = (async () => await stocksSellOrdersService.CreateSellOrder(null, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Theory]
        [InlineData(100001)]
        public async Task CreateSellOrder_QuantityIsMoreThanMaximum(uint sellOrderQuantity)
        {
            var request = fixture.Build<SellOrderRequest>()
                .With(r => r.Quantity, sellOrderQuantity)
                .Create();
            var action = (async () => await stocksSellOrdersService.CreateSellOrder(null, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_PriceIsLessThanMinimum(uint sellOrderPrice)
        {
            var request = fixture.Build<SellOrderRequest>()
                .With(r => r.Price, sellOrderPrice)
                .Create();
            var action = (async () => await stocksSellOrdersService.CreateSellOrder(null, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Theory]
        [InlineData(10001)]
        public async Task CreateSellOrder_PriceIsMoreThanMaximum(uint sellOrderPrice)
        {
            var request = fixture.Build<SellOrderRequest>()
                .With(r => r.Price, sellOrderPrice)
                .Create();
            var action = (async () => await stocksSellOrdersService.CreateSellOrder(null, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull()
        {
            var request = fixture.Build<SellOrderRequest>()
                .With(r => r.StockSymbol, null as string)
                .Create();
            var action = (async () => await stocksSellOrdersService.CreateSellOrder(null, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateSellOrder_DateOfOrderIsLessThanYear2000()
        {
            var request = fixture.Build<SellOrderRequest>()
                .With(r => r.DateAndTimeOfOrder, new DateTime(1999, 12, 31))
                .Create();
            var action = (async () => await stocksSellOrdersService.CreateSellOrder(null, ""));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateSellOrder_ValidRequest()
        {
            var request = fixture.Create<SellOrderRequest>();
            var order = request.ToSellOrder();
            mock.Setup(r => r.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(order);
            var expected = order.ToSellOrderResponse();

            var actual = await stocksSellOrdersService.CreateSellOrder(request, "");
            expected.SellOrderID = actual.SellOrderID;

            actual.SellOrderID.Should().NotBeEmpty();
            actual.Should().Be(expected);
        }
        #endregion
        #region GetBuyOrders
        [Fact]
        public async Task GetBuyOrders_EmptyList()
        {
            mock.Setup(r => r.GetBuyOrders("")).ReturnsAsync(new List<BuyOrder>());

            var collection = await stocksBuyOrdersService.GetBuyOrders("");

            collection.Should().NotBeNull().And.BeEmpty();
        }
        [Fact]
        public async Task GetBuyOrders_ValidRequest()
        {
            var orders = fixture.Create<List<BuyOrder>>();
            mock.Setup(r => r.GetBuyOrders("")).ReturnsAsync(orders);
            var expected = orders.Select(o => o.ToBuyOrderResponse());

            var actual = await stocksBuyOrdersService.GetBuyOrders("");

            actual.Count.Should().Be(expected.Count());
            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
        #region GetSellOrders
        [Fact]
        public async Task GetSellOrders_EmptyList()
        {
            mock.Setup(r => r.GetSellOrders("")).ReturnsAsync(new List<SellOrder>());

            var collection = await stocksSellOrdersService.GetSellOrders("");

            collection.Should().NotBeNull().And.BeEmpty();
        }
        [Fact]
        public async Task GetSellOrders_ValidRequest()
        {
            var orders = fixture.Create<List<SellOrder>>();
            mock.Setup(r => r.GetSellOrders("")).ReturnsAsync(orders);
            var expected = orders.Select(o => o.ToSellOrderResponse());

            var actual = await stocksSellOrdersService.GetSellOrders("");

            actual.Count.Should().Be(expected.Count());
            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }

}
