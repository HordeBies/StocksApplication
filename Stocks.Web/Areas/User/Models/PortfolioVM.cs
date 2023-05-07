using Stocks.Core.DTO;

namespace Stocks.Web.Areas.User.Models
{
    public class PortfolioVM
    {
        public string FullName { get; set; }
        public double Balance { get; set; }
        public List<UserStockVM> Stocks { get; set; }
    }
}
