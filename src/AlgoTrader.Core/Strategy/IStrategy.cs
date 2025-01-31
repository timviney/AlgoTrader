using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Core.MarketData;
using AlgoTrader.Core.Trades;

namespace AlgoTrader.Core.Strategy
{
    public interface IStrategy
    {
        void NextPeriod(CurrentPrice currentPrice, MarketDataPoint previousPeriod);
        void End();
        TradingResults GetResults();
    }
}
