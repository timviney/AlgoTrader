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
        private readonly TradeCollection _trades = new();
        private readonly PositionCollection _positions = new();

        public Trade RecordTrade(TradeDirection direction, Symbol symbol, decimal quantity, decimal price, DateTime dateTime, Position position)
        {
            var trade = new Trade(symbol, direction, quantity, price, dateTime, position);
            _trades.Record(trade);
            position.Trades.Add(trade);
            position.Status = trade.Direction == position.Direction
                ? position.Status
                : Math.Abs(position.Quantity()) < Constants.Tol
                    ? PositionStatus.Closed
                    : PositionStatus.PartiallyClosed;
            return trade;
        }

        public Position OpenPosition(TradeDirection direction, Symbol symbol)
        {
            var position = new Position(direction, symbol);
            _positions.Record(position);
            return position;
        }

        public void ClosePosition(Position position, decimal price, DateTime currentDateTime)
        {
            RecordTrade(position.Direction.Opposite(), position.Symbol, position.Quantity(), price, currentDateTime, position);
        }

        public List<Position> GetAllPositions() => _positions.ToList();

        public bool TryGetOpenPositions(Symbol symbol, out List<Position>? positions)
        {
            positions = _positions.GetOpenPositionsOrNull(symbol);
            return positions != null;
        }

    }
}
