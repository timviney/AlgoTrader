using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Core.Strategy;
using AlgoTrader.Core.Trades;

namespace AlgoTrader.Core.BollingerBands
{
    internal class BollingerBandsExecutor(TradingInputs tradingInputs, InputsBollingerBands strategyInputs)
        : StrategyExecutor<InputsBollingerBands>(tradingInputs, strategyInputs)
    {
        private bool _wasBelowLower;
        private bool _wasAboveUpper;

        protected sealed override void Run()
        {
            if (MarketState.NumberOfRecordedPeriods < StrategyInputs.Period) return;

            // Calculate Bollinger Bands components
            var middleBand = MarketState.PriceAverage(StrategyInputs.Period);
            var previousPrices = MarketState.LastNPeriods(StrategyInputs.Period);

            var sumOfSquares = previousPrices.Sum(price => (price.Close - middleBand) * (price.Close - middleBand));

            var standardDeviation = (decimal)Math.Sqrt((double)(sumOfSquares / StrategyInputs.Period));
            var upperBand = middleBand + standardDeviation * StrategyInputs.StandardDeviationMultiplier;
            var lowerBand = middleBand - standardDeviation * StrategyInputs.StandardDeviationMultiplier;

            var currentPrice = MarketState.Current.Price;

            // Check for buy signal (price moves below lower band)
            if (currentPrice < lowerBand)
            {
                if (!_wasBelowLower)
                {
                    RecordTrade(TradeDirection.Buy, StrategyInputs.MaximumBuy);
                    _wasBelowLower = true;
                }
            }
            else
            {
                _wasBelowLower = false;
            }

            // Check for sell signal (price moves above upper band)
            if (currentPrice > upperBand)
            {
                if (!_wasAboveUpper)
                {
                    RecordTrade(TradeDirection.Sell, StrategyInputs.MaximumSell);
                    _wasAboveUpper = true;
                }
            }
            else
            {
                _wasAboveUpper = false;
            }
        }
    }
}
