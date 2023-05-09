using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.DTO;
using Stocks.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<ApplicationUser> UpdateBalance(BuyOrderRequest request, string userId)
        {
            var user = await userRepository.GetUser(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.Balance -= request.Price * request.Quantity;
            return await userRepository.UpdateBalance(user);
        }
        public async Task<ApplicationUser> UpdateBalance(SellOrderRequest request, string userId)
        {
            var user = await userRepository.GetUser(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.Balance += request.Price * request.Quantity;
            return await userRepository.UpdateBalance(user);
        }
    }
}
