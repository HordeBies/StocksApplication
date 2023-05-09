using Stocks.Core.DTO;

namespace Stocks.Web.Areas.User.Models
{
    public class OrderHistoryVM
    {
        public IEnumerable<SellOrderResponse> SellOrders { get; set; }
        public IEnumerable<BuyOrderResponse> BuyOrders { get; set; }
        public string? Symbol { get; set; }
    }
}
