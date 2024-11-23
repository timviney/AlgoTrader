using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    public record Trade(Symbol Symbol, TradeDirection Direction, decimal Quantity, decimal Price, DateTime DateTime)
    {
        public decimal Profit = Price * Quantity * (Direction == TradeDirection.Buy ? -1 : 1);
    }
}
