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

        private Trade RecordTrade(TradeDirection direction, Symbol symbol, decimal quantity, decimal price, DateTime dateTime, TradeStatus status)
        {
            var trade = new Trade(symbol, direction, quantity, price, dateTime, status);
            _trades.Add(trade);
            return trade;
        }

        public void OpenPosition(Symbol symbol, decimal quantity, decimal price, DateTime dateTime)
        {
            RecordTrade(TradeDirection.Buy, symbol, quantity, price, dateTime, TradeStatus.Open);
        }

        public void ClosePosition(Trade buyTrade, decimal quantity, decimal price, DateTime dateTime)
        {
            var sellTrade = RecordTrade(TradeDirection.Sell, buyTrade.Symbol, quantity, price, dateTime, TradeStatus.Closed);

            buyTrade.Status = (buyTrade.Quantity <= quantity + Constants.Tol)
                ? TradeStatus.Closed
                : TradeStatus.PartiallyClosed;

            sellTrade.PairedTrades.Add(buyTrade);
            buyTrade.PairedTrades.Add(sellTrade);
        }

        public List<Trade> OpenPositions(Symbol symbol)
        {
            var trades = _trades[symbol];
            if (trades == null || trades.Count == 0) return [];

            return trades.Where(t => t.Status != TradeStatus.Closed).ToList();
        }

        public List<Trade> GetAllTrades() => _trades.ToList();
    }
}
