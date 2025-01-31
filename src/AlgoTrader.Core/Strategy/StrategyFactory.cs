using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;
using AlgoTrader.Core.BollingerBands;
using AlgoTrader.Core.MovingAverageCrossover;
using AlgoTrader.Core.Trades;

namespace AlgoTrader.Core.Strategy
{
    public static class StrategyFactory
    {
        public static IStrategy Get(TradingInputs tradingInputs, IStrategyInputs strategyInputs)
        {
            return strategyInputs switch
            {
                InputsMovingAverageCrossover maInputs => new MovingAverageCrossoverExecutor(tradingInputs, maInputs),
                InputsBollingerBands bbInputs => new BollingerBandsExecutor(tradingInputs, bbInputs),
                _ => throw new ArgumentOutOfRangeException(nameof(strategyInputs))
            };
        }
    }
}
