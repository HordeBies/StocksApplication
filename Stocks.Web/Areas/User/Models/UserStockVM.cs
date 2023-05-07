namespace Stocks.Web.Areas.User.Models
{
    public class UserStockVM
    {
        public string StockName { get; set; }
        public string StockSymbol { get; set; }
        public int SharesOwned { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalValue { get; set; }
        public decimal PercentChange { get; set; }
        public DateTime LastOrder { get; set; }
    }
}
