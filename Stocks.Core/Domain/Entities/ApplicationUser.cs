using Microsoft.AspNetCore.Identity;

namespace Stocks.Core.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public double Balance { get; set; }
        // Navigation properties
        public List<BuyOrder> BuyOrders { get; set; }
        public List<SellOrder> SellOrders { get; set; }
        public List<UserStock> Stocks { get; set; }
    }
}
