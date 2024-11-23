using System.Linq.Expressions;
using AlgoTrader.AlphaVantage;
using AlgoTrader.Common;
using AlgoTrader.Core;

namespace AlgoTrader.Historic.Engine
{
    public static class Engine
    {
        public static async Task<TradingResults> Run(DateTime from, DateTime to, Symbol symbol, IStrategyInputs inputs)
        {
            var strategy = StrategyFactory.Get(inputs);

            var dataClient = new AlphaVantageClient();

            var historicIntradayData = await dataClient.GetIntradayDataAsync(symbol, Interval._5min);

            foreach ((DateTime dateTime, IntradayTimeSeries data) in historicIntradayData.TimeSeries)
            {
                var marketDataPoint = new MarketDataPoint(dateTime, data.Open, data.High, data.Low, data.Close, data.Volume);

                strategy.NextPeriod(marketDataPoint);
            }

            return strategy.GetResults();
        }
    }
}
