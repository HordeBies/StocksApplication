using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Core.Domain.RepositoryContracts
{
    public interface IStockMarketDbInitializer
    {
        Task Initialize();
    }
}
