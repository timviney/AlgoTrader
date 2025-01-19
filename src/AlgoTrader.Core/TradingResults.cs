using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    public record TradingResults(List<Position> Positions, List<MarketDataPoint> Prices)
    {
        public decimal Profit() => Positions.Sum(p => p.Trades.Sum(t => t.Profit));
        public List<Trade> Trades => Positions.SelectMany(p => p.Trades).ToList();
        public List<Trade> Buys => Positions.SelectMany(p => p.Trades.Where(t => t.Direction == TradeDirection.Buy)).ToList();
        public List<Trade> Sells => Positions.SelectMany(p => p.Trades.Where(t => t.Direction == TradeDirection.Sell)).ToList();
    }
}
