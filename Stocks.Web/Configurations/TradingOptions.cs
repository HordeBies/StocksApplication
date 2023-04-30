namespace Stocks.Web.Configurations
{
    public class TradingOptions
    {
        public uint? DefaultOrderQuantity { get; set; }
        public List<string>? Top25PopularStocks { get; set; }
    }
}
