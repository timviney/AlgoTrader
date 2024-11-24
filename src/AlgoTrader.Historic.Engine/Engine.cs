using System.Linq.Expressions;
using AlgoTrader.AlphaVantage;
using AlgoTrader.Common;
using AlgoTrader.Core;

namespace AlgoTrader.Historic.Engine
{
    public static class Engine
    {
        public static async Task<TradingResults> Run(DateTime from, DateTime to, TradingInputs tradingInputs, IStrategyInputs strategyInputs)
        {
            var strategy = StrategyFactory.Get(tradingInputs, strategyInputs);

            var dataClient = new AlphaVantageClient();

            var historicIntradayData = await dataClient.GetIntradayDataAsync(tradingInputs.Symbol, Interval._5min, from, to);

            foreach ((DateTime dateTime, IntradayTimeSeries data) in historicIntradayData)
            {
                var marketDataPoint = new MarketDataPoint(dateTime, data.Open, data.High, data.Low, data.Close, data.Volume);

                strategy.NextPeriod(marketDataPoint);
            }

            return strategy.GetResults();
        }
    }
}
