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

        private void RecordTrade(TradeDirection direction, Symbol symbol, double quantity, double price, DateTime dateTime)
        {
            var trade = new Trade(symbol, direction, quantity, price, dateTime);
            _trades.Add(trade);
        }

        public void Buy(Symbol symbol, double quantity, double price, DateTime dateTime) => RecordTrade(TradeDirection.Buy, symbol, quantity, price, dateTime);
        public void Sell(Symbol symbol, double quantity, double price, DateTime dateTime) => RecordTrade(TradeDirection.Sell, symbol, quantity, price, dateTime);

        public double OpenPosition(Symbol symbol) => _trades[symbol].Sum(t => t.Quantity);

        public List<Trade> GetAllTrades() => _trades.ToList();
    }
}
