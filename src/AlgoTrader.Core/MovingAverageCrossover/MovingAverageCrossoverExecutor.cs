using AlgoTrader.Common;

namespace AlgoTrader.Core.MovingAverageCrossover
{
    internal class MovingAverageCrossoverExecutor(TradingInputs tradingInputs, InputsMovingAverageCrossover strategyInputs) : StrategyExecutor<InputsMovingAverageCrossover>(tradingInputs, strategyInputs)
    {
        private bool? WasBullish;
        protected sealed override void Run()
        {
            if (MarketState.NumberOfRecordedPeriods < StrategyInputs.LongTerm) return;

            // Calculate moving averages and trading signals

            var shortTermAvg = MarketState.PriceAverage(StrategyInputs.ShortTerm);
            var longTermAvg = MarketState.PriceAverage(StrategyInputs.LongTerm);

            bool bullish = shortTermAvg > longTermAvg;

            if (WasBullish != null && bullish != WasBullish) // We have crossover, either Golden or Death
            {
                // Account for some threshold to avoid overzealous behaviour on variable prices
                // TODO this crossover will cause issues because once skipped it will never check again
                if (shortTermAvg > longTermAvg * (1 + StrategyInputs.CrossoverThreshold))
                {
                    RecordTrade(TradeDirection.Buy, StrategyInputs.MaximumBuy);
                }
                else if (shortTermAvg < longTermAvg * (1 - StrategyInputs.CrossoverThreshold))
                {
                    RecordTrade(TradeDirection.Sell, StrategyInputs.MaximumSell);
                }
            }

            WasBullish = bullish;
        }
    }
}
