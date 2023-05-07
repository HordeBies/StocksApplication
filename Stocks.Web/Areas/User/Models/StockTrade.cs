namespace Stocks.Web.Areas.User.Models
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public double CurrentPrice { get; set; }
        public double PreviousClosedPrice { get; set; }
        public uint Quantity { get; set; }
        public double ChangePercent { get; set; }// In percentage
        public double DailyChange { get; set; }// In USD
        public int SharesOwned { get; set; }
    }
}
