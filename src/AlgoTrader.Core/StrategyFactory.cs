using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;
using AlgoTrader.Core.MovingAverageCrossover;

namespace AlgoTrader.Core
{
    public static class StrategyFactory
    {
        public static IStrategy Get(IStrategyInputs inputs)
        {
            return inputs switch
            {
                Inputs maInputs => new Strategy(maInputs),
                _ => throw new ArgumentOutOfRangeException(nameof(inputs))
            };
        }
    }
}
