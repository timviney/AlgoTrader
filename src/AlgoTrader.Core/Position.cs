using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    public class Position(TradeDirection direction, Symbol symbol)
    {
        public int Id { get; set; }
        public TradeDirection Direction { get; init; } = direction;
        public Symbol Symbol { get; init; } = symbol;
        public PositionStatus Status { get; set; } = PositionStatus.Open;
        public List<Trade> Trades { get; private set; } = [];
        public decimal Profit() => Trades.Sum(t => t.Profit);
        public decimal Quantity() => Trades.Sum(t => t.Quantity * (t.Direction == Direction ? 1 : -1));
    }
}
