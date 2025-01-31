using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core.MarketData
{
    public record CurrentPrice(DateTime DateTime, decimal Price)
    {
    }
}
