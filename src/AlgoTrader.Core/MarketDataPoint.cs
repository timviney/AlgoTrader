using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    public record MarketDataPoint(DateTime DateTime, decimal Open, decimal High, decimal Low, decimal Close, int Volume)
    {
    }
}
