using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core.Trades
{
    public record TradingInputs(Symbol Symbol, decimal Slippage)
    {
    }
}
