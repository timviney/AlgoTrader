using AlgoTrader.Common;

namespace AlgoTrader.Core.MovingAverageCrossover
{
    internal class Strategy(TradingInputs tradingInputs, Inputs strategyInputs) : Strategy<Inputs>(tradingInputs, strategyInputs)
    {
        protected sealed override void Run()
        {
            if (MarketState.NumberOfRecordedPeriods < StrategyInputs.LongTerm) return;

            // Calculate moving averages and trading signals

            var shortTermAvg = MarketState.PriceAverage(StrategyInputs.ShortTerm);
            var longTermAvg = MarketState.PriceAverage(StrategyInputs.LongTerm);

            if (shortTermAvg > longTermAvg * (1 + StrategyInputs.CrossoverThreshold))
            {
                RecordTrade(TradeDirection.Buy, StrategyInputs.MaximumBuy);
            }
            else if (shortTermAvg < longTermAvg * (1 - StrategyInputs.CrossoverThreshold))
            {
                RecordTrade(TradeDirection.Sell, StrategyInputs.MaximumSell);
            }
        }
    }
}
