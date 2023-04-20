
namespace ServiceContracts.DTO
{
    public interface IOrderResponse
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }
        public OrderType TypeOfOrder { get; }
    }
    public enum OrderType
    {
        BuyOrder,
        SellOrder
    }
}
