using AlgoTrader.Common;
using AlgoTrader.Core.Strategy;
using AlgoTrader.Core.Trades;

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
                if (shortTermAvg > longTermAvg)
                {
                    RecordTrade(TradeDirection.Buy, StrategyInputs.MaximumBuy, StrategyInputs.MaxExposure);
                }
                else if (shortTermAvg < longTermAvg)
                {
                    RecordTrade(TradeDirection.Sell, StrategyInputs.MaximumSell, StrategyInputs.MaxExposure);
                }
            }

            WasBullish = bullish;
        }
    }
}
