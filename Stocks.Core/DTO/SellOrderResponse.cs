using Stocks.Core.Domain.Entities;
using Stocks.Core.Enums;

namespace Stocks.Core.DTO
{
    public class SellOrderResponse : IOrderResponse
    {
        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }
        public OrderType TypeOfOrder { get; } = OrderType.SellOrder;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(SellOrderResponse)) return false;
            var other = (SellOrderResponse)obj;
            return other.SellOrderID == SellOrderID
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
    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                TradeAmount = sellOrder.Quantity * sellOrder.Price
            };
        }
    }
}
