using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    public class Trade(Symbol symbol, TradeDirection direction, decimal quantity, decimal price, DateTime dateTime, TradeStatus status)
    {
        public int Id { get; set; } = -1;
        public Symbol Symbol { get; init; } = symbol;
        public TradeDirection Direction { get; init; } = direction;
        public decimal Quantity { get; init; } = quantity;
        public decimal Price { get; init; } = price;
        public DateTime DateTime { get; init; } = dateTime;
        public List<Trade> PairedTrades { get; set; } = [];
        public TradeStatus Status { get; set; } = status;

        public decimal Profit = price * quantity * (direction == TradeDirection.Buy ? -1 : 1);

        public decimal OpenQuantity()
        {
            return Status switch
            {
                TradeStatus.Open => Quantity,
                TradeStatus.PartiallyClosed => Quantity - PairedTrades.Sum(t => t.Quantity),
                TradeStatus.Closed => 0,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
