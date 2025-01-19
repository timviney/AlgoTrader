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
        public static IStrategy Get(TradingInputs tradingInputs, IStrategyInputs strategyInputs)
        {
            return strategyInputs switch
            {
                InputsMovingAverageCrossover maInputs => new MovingAverageCrossoverExecutor(tradingInputs, maInputs),
                _ => throw new ArgumentOutOfRangeException(nameof(strategyInputs))
            };
        }
    }
}
