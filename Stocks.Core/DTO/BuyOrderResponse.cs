using Stocks.Core.Domain.Entities;
using Stocks.Core.Enums;

namespace Stocks.Core.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }
        public OrderType TypeOfOrder { get; } = OrderType.BuyOrder;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(BuyOrderResponse)) return false;
            var other = (BuyOrderResponse)obj;
            return other.BuyOrderID == BuyOrderID
                && other.StockSymbol == StockSymbol
                && other.StockName == StockName
                && other.DateAndTimeOfOrder == DateAndTimeOfOrder
                && other.Quantity == Quantity
                && other.Price == Price
                && other.TradeAmount == TradeAmount;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                TradeAmount = buyOrder.Quantity * buyOrder.Price
            };
        }
    }
}
