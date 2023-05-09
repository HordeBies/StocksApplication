using Stocks.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.DTO
{
    public class SellOrderRequest : IOrderRequest, IValidatableObject
    {
        [Required]
        public string StockSymbol { get; set; }
        [Required]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint Quantity { get; set; }
        [Range(1, 10000)]
        public double Price { get; set; }

        public SellOrder ToSellOrder()
        {
            return new SellOrder
            {
                SellOrderID = Guid.NewGuid(),
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price
            };
        }
        public UserStock ToUserStock(string userId)
        {
            return new UserStock
            {
                ApplicationUserId = userId,
                StockId = StockSymbol,
                Amount = (int)Quantity,
                Cost = Price * Quantity,
                LastChange = DateTime.Now,
            };
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            //Date of order should be later than Jan 01, 2000
            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000."));
            }

            return results;
        }
    }
}
