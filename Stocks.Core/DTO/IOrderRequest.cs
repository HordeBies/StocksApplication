
namespace Stocks.Core.DTO
{
    public interface IOrderRequest
    {
        string StockSymbol { get; set; }
        string StockName { get; set; }
        DateTime DateAndTimeOfOrder { get; set; }
        uint Quantity { get; set; }
        double Price { get; set; }
    }
}
