using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stocks.Core.Domain.Entities
{
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }
        [Required]
        [StringLength(10)]
        public string StockSymbol { get; set; }
        [Required]
        [StringLength(50)]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint Quantity { get; set; }
        [Range(1, 10000)]
        public double Price { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
