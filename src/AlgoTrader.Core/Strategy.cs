using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    public abstract class Strategy<TInputs>(TradingInputs tradingInputs, TInputs strategyInputs) : IStrategy
        where TInputs : IStrategyInputs
    {
        protected TradingInputs TradingInputs { get; } = tradingInputs;
        protected TInputs StrategyInputs { get; } = strategyInputs;

        internal MarketState MarketState { get; } = new();
        internal TradingState TradingState { get; } = new();

        protected void RecordTrade(TradeDirection direction, decimal quantity)
        {
            var price = MarketState.OpeningPrice(direction, TradingInputs.Slippage);
            TradingState.RecordTrade(direction, TradingInputs.Symbol, quantity, price, MarketState.Current.DateTime);
        }

        protected abstract void Run();

        public void NextPeriod(MarketDataPoint marketDataPoint)
        {
            MarketState.Update(marketDataPoint);

            Run();
        }

        public TradingResults GetResults() => new(TradingState.GetAllTrades(), MarketState.Data);
    }
}
