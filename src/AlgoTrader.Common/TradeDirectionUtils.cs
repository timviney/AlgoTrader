using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Core;

namespace AlgoTrader.Common
{
    public static class TradeDirectionUtils
    {
        public static TradeDirection Opposite(this TradeDirection direction)
        {
            return direction == TradeDirection.Buy ? TradeDirection.Sell : TradeDirection.Buy;
        }

        public static int Multiplier(this TradeDirection direction)
        {
            return direction == TradeDirection.Buy ? 1 : -1;
        }
    }
}
