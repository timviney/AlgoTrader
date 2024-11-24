using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    public record TradingResults(List<Trade> Trades, List<MarketDataPoint> Prices)
    {
        public decimal Profit() => Trades.Sum(t => t.Profit);
        public List<Trade> Buys => Trades.Where(t => t.Direction == TradeDirection.Buy).ToList();
        public List<Trade> Sells => Trades.Where(t => t.Direction == TradeDirection.Sell).ToList();
    }
}
