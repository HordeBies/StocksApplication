using Stocks.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Core.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        public Task<ApplicationUser> UpdateBalance(ApplicationUser user);
        public Task<ApplicationUser> GetUser(string userId);
    }
}
