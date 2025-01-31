using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core.Trades
{
    public class Trade(Symbol symbol, TradeDirection direction, decimal quantity, decimal price, DateTime dateTime, Position position)
    {
        public int Id { get; set; } = -1;
        public Symbol Symbol { get; init; } = symbol;
        public TradeDirection Direction { get; init; } = direction;
        public decimal Quantity { get; init; } = quantity;
        public decimal Price { get; init; } = price;
        public DateTime DateTime { get; init; } = dateTime;
        public Position Position { get; set; } = position;

        public decimal Profit = price * quantity * -direction.Multiplier();
    }
}
