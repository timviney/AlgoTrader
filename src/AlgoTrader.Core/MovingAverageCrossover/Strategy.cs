using AlgoTrader.Common;

namespace AlgoTrader.Core.MovingAverageCrossover
{
    internal class Strategy(TradingInputs tradingInputs, Inputs strategyInputs) : Strategy<Inputs>(tradingInputs, strategyInputs)
    {
        protected sealed override void Run()
        {
            if (MarketState.NumberOfRecordedPeriods < StrategyInputs.LongTerm) return;

            // Calculate moving averages and trading signals

            decimal shortTermAvg = MarketState.PriceAverage(StrategyInputs.ShortTerm);
            decimal longTermAvg = MarketState.PriceAverage(StrategyInputs.LongTerm);

            if (shortTermAvg > longTermAvg)
            {
                // BUY
                RecordTrade(TradeDirection.Buy, StrategyInputs.MaximumBuy);
            }
            else if (shortTermAvg < longTermAvg)
            {
                // SELL
                var openPosition = TradingState.OpenPosition(TradingInputs.Symbol);
                if (openPosition <= Constants.Tol) return;

                var availableToBuy = Math.Min(StrategyInputs.MaximumSell, openPosition);

                RecordTrade(TradeDirection.Sell, availableToBuy);
            }
        }
    }
}
