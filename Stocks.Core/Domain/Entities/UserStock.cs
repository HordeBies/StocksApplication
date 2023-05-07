using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Core.Domain.Entities
{
    public class UserStock
    {
        public string ApplicationUserId { get; set; }
        public string StockId { get; set; }
        public int Amount { get; set; }
    }
}
