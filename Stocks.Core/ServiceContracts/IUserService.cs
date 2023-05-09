using Stocks.Core.Domain.Entities;
using Stocks.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Core.ServiceContracts
{
    public interface IUserService
    {
        public Task<ApplicationUser> UpdateBalance(BuyOrderRequest request, string userId);
        public Task<ApplicationUser> UpdateBalance(SellOrderRequest request, string userId);
    }
}
