using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    public record TradingInputs(Symbol Symbol, decimal Slippage)
    {
    }
}
