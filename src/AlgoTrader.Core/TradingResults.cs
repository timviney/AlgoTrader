using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    public record TradingResults(List<Trade> Trades, List<MarketDataPoint> Prices)
    {
        public double Profit() => Trades.Sum(t => t.Profit);
    }
}
