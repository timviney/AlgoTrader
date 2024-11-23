using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    public abstract class Strategy<TInputs>(TInputs inputs) : IStrategy
        where TInputs : IStrategyInputs
    {
        public IStrategyInputs Inputs { get; } = inputs;

        internal MarketState MarketState { get; } = new();
        internal TradingState TradingState { get; } = new();
        protected abstract void Run();

        public void NextPeriod(MarketDataPoint marketDataPoint)
        {
            MarketState.Update(marketDataPoint);

            Run();
        }

        public TradingResults GetResults() => new(TradingState.GetAllTrades(), MarketState.Data);
    }
}
