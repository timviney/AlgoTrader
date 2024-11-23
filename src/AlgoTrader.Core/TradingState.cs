using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    internal class TradingState
    {
        private readonly TradeList _trades = new();

        public void RecordTrade(TradeDirection direction, Symbol symbol, decimal quantity, decimal price, DateTime dateTime)
        {
            var trade = new Trade(symbol, direction, quantity, price, dateTime);
            _trades.Add(trade);
        }

        public decimal OpenPosition(Symbol symbol) => _trades[symbol]?.Sum(t => t.Quantity)??0;

        public List<Trade> GetAllTrades() => _trades.ToList();
    }
}
