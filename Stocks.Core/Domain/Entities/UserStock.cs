
namespace Stocks.Core.Domain.Entities
{
    public class UserStock
    {
        public string ApplicationUserId { get; set; }
        public string StockId { get; set; }
        public int Amount { get; set; } // Number of shares
        public double Cost { get; set; } // Money spent on shares
        public DateTime LastChange { get; set; }
    }
}
